using MediatR;
using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterLotesProvaPorData;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvasFinalizadasPorData;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UseCases
{
    public class BuscarProvasFinalizadasUseCaseTeste
    {
        private readonly BuscarProvasFinalizadasUseCase buscarProvasFinalizadasUseCase;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IChannel> channel;
        private readonly Mock<IServicoLog> serviceLog;

        public BuscarProvasFinalizadasUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            channel = new Mock<IChannel>();
            serviceLog = new Mock<IServicoLog>();
            buscarProvasFinalizadasUseCase = new BuscarProvasFinalizadasUseCase(mediator.Object, channel.Object, serviceLog.Object);
        }

        [Fact]
        public async Task Deve_Buscar_Provas_Finalizadas()
        {
            var mensagemRabbit = new MensagemRabbit(string.Empty, Guid.NewGuid());
            var provasDtos = ObterProvasDtos();

            mediator.Setup(m => m.Send(It.IsAny<ObterProvasFinalizadasPorDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(provasDtos);

            mediator.Setup(m => m.Send(It.IsAny<InserirLoteProvaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(m => m.Send(It.IsAny<ObterLotesProvaPorDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<LoteProva>
                {
                    new LoteProva { Id = 3 },
                });

            mediator.Setup(m => m.Send(It.IsAny<DesativarTodosLotesProvaAtivosCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await buscarProvasFinalizadasUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(provasDtos.Count));
        }

        [Fact]
        public async Task Nao_Deve_Publicar_Buscar_Alunos_Prova_Proficiencia_Boletim()
        {
            var mensagemRabbit = new MensagemRabbit(string.Empty, Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<ObterProvasFinalizadasPorDataQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<ProvaDto>>(null!));

            mediator.Setup(m => m.Send(It.IsAny<InserirLoteProvaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(m => m.Send(It.IsAny<ObterLotesProvaPorDataQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<LoteProva>>(null!));

            mediator.Setup(m => m.Send(It.IsAny<DesativarTodosLotesProvaAtivosCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await buscarProvasFinalizadasUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_False_Caso_Excecao()
        {
            var mensagemRabbit = new MensagemRabbit(string.Empty, Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<ObterProvasFinalizadasPorDataQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro teste"));

            var resultado = await buscarProvasFinalizadasUseCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            serviceLog.Verify(m => m.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        private static List<ProvaDto> ObterProvasDtos()
        {
            return new List<ProvaDto>
            {
                new ProvaDto { Id = 1, FormatoTai = false, Inicio = DateTime.Now },
                new ProvaDto { Id = 2, FormatoTai = true, Inicio = DateTime.Now }
            };
        }
    }
}
