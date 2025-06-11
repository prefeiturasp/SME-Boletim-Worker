using MediatR;
using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.LoteProva.AlterarStatusConsolidacao;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimLoteProvaPendentes;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UsesCases
{
    public class BuscarProvasBoletimLoteUseCaseTeste
    {
        private readonly BuscarProvasBoletimLoteUseCase buscarProvasBoletimLoteUseCase;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IChannel> channel;
        private readonly Mock<IServicoLog> serviceLog;

        public BuscarProvasBoletimLoteUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            channel = new Mock<IChannel>();
            serviceLog = new Mock<IServicoLog>();
            buscarProvasBoletimLoteUseCase = new BuscarProvasBoletimLoteUseCase(mediator.Object, channel.Object, serviceLog.Object);
        }

        [Fact]
        public async Task Deve_Buscar_Provas_Boletim_Lote()
        {
            var mensagemRabbit = new MensagemRabbit(string.Empty, Guid.NewGuid());

            var boletinsLotesProvas = ObterBoletisLotesProvas();

            mediator.Setup(m => m.Send(It.IsAny<ObterBoletimLoteProvaPendentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(boletinsLotesProvas);

            mediator.Setup(m => m.Send(It.IsAny<AlterarLoteProvaStatusConsolidacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await buscarProvasBoletimLoteUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(boletinsLotesProvas.Count * 2));
        }

        [Fact]
        public async Task Nao_Deve_Publicar_Filas_Consolidacao_Boletim_Lote()
        {
            var mensagemRabbit = new MensagemRabbit(string.Empty, Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<ObterBoletimLoteProvaPendentesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<BoletimLoteProva>>(null!));

            mediator.Setup(m => m.Send(It.IsAny<AlterarLoteProvaStatusConsolidacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await buscarProvasBoletimLoteUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private static List<BoletimLoteProva> ObterBoletisLotesProvas()
        {
            return new List<BoletimLoteProva>
            {
                new BoletimLoteProva { LoteId = 1, ProvaId = 1},
                new BoletimLoteProva { LoteId = 1, ProvaId = 2}
            };
        }
    }
}
