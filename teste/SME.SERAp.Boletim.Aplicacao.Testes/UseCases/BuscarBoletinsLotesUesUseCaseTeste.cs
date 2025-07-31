using MediatR;
using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaPorId;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;
using System.Text.Json;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UseCases
{
    public class BuscarBoletinsLotesUesUseCaseTeste
    {
        private readonly BuscarBoletinsLotesUesUseCase buscarBoletinsLotesUesUseCase;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IChannel> channel;
        private readonly Mock<IServicoLog> serviceLog;

        public BuscarBoletinsLotesUesUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            channel = new Mock<IChannel>();
            serviceLog = new Mock<IServicoLog>();
            buscarBoletinsLotesUesUseCase = new BuscarBoletinsLotesUesUseCase(mediator.Object, channel.Object, serviceLog.Object);
        }

        [Fact]
        public async Task Deve_Buscar_Boletins_Lotes_Ues()
        {
            var loteId = 1L;
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(loteId), Guid.NewGuid());
            var uesBoletinsLotes = ObterBoletinsLotesUes(loteId);

            mediator.Setup(m => m.Send(It.IsAny<ObterUesTotalAlunosPorLoteIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesBoletinsLotes);

            var uesRealizaramProva = ObterBoletinsLotesUesRealizaramProva(loteId);
            mediator.Setup(m => m.Send(It.IsAny<ObterUesAlunosRealizaramProvaPorLoteIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesRealizaramProva);

            var resultado = await buscarBoletinsLotesUesUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(uesBoletinsLotes.Count()));
            mediator.Verify(m => m.Send(It.IsAny<ObterUesTotalAlunosPorLoteIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterUesAlunosRealizaramProvaPorLoteIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            serviceLog.Verify(x => x.Registrar(It.IsAny<Exception>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_LoteId_Invalido()
        {
            var loteId = 1L;
            var mensagemRabbit = new MensagemRabbit(string.Empty, Guid.NewGuid());
            var uesBoletinsLotes = ObterBoletinsLotesUes(loteId);

            mediator.Setup(m => m.Send(It.IsAny<ObterUesTotalAlunosPorLoteIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesBoletinsLotes);

            var uesRealizaramProva = ObterBoletinsLotesUesRealizaramProva(loteId);
            mediator.Setup(m => m.Send(It.IsAny<ObterUesAlunosRealizaramProvaPorLoteIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesRealizaramProva);

            var resultado = await buscarBoletinsLotesUesUseCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ObterUesTotalAlunosPorLoteIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ObterUesAlunosRealizaramProvaPorLoteIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            serviceLog.Verify(x => x.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Buscar_Boletins_Lotes_Ues()
        {
            var loteId = 1L;
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(loteId), Guid.NewGuid());
            mediator.Setup(m => m.Send(It.IsAny<ObterUesTotalAlunosPorLoteIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BoletimLoteUe>());

            var uesRealizaramProva = ObterBoletinsLotesUesRealizaramProva(loteId);
            mediator.Setup(m => m.Send(It.IsAny<ObterUesAlunosRealizaramProvaPorLoteIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uesRealizaramProva);

            var resultado = await buscarBoletinsLotesUesUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ObterUesTotalAlunosPorLoteIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterUesAlunosRealizaramProvaPorLoteIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            serviceLog.Verify(x => x.Registrar(It.IsAny<Exception>()), Times.Never);
        }

        private IEnumerable<BoletimLoteUe> ObterBoletinsLotesUes(long loteId)
        {
            return new List<BoletimLoteUe>() 
            { 
                new BoletimLoteUe(1, 2, loteId, 5, 10, 0),
                new BoletimLoteUe(3, 4, loteId, 5, 10, 0),
                new BoletimLoteUe(5, 6, loteId, 5, 10, 0),
            };
        }

        private IEnumerable<BoletimLoteUeRealizaramProvaDto> ObterBoletinsLotesUesRealizaramProva(long loteId)
        {
            return new List<BoletimLoteUeRealizaramProvaDto>()
            {
                new BoletimLoteUeRealizaramProvaDto { DreId = 1, UeId = 2, LoteId = loteId, AnoEscolar = 5, RealizaramProva = 8 },
                new BoletimLoteUeRealizaramProvaDto { DreId = 3, UeId = 4, LoteId = loteId, AnoEscolar = 5, RealizaramProva = 8 },
                new BoletimLoteUeRealizaramProvaDto { DreId = 5, UeId = 6, LoteId = loteId, AnoEscolar = 5, RealizaramProva = 8 },
            };
        }
    }
}
