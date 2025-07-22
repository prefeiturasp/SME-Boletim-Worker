using MediatR;
using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;
using System.Text.Json;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UseCases
{
    public class TratarBoletimLoteUeUseCaseTeste
    {
        private readonly TratarBoletimLoteUeUseCase tratarBoletimLoteUeUseCase;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IChannel> channel;
        private readonly Mock<IServicoLog> serviceLog;
        public TratarBoletimLoteUeUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            channel = new Mock<IChannel>();
            serviceLog = new Mock<IServicoLog>();
            tratarBoletimLoteUeUseCase = new TratarBoletimLoteUeUseCase(mediator.Object, channel.Object, serviceLog.Object);
        }

        [Fact]
        public async Task Deve_Inserir_BoletimLoteUe()
        {
            var boletimLoteUe = new BoletimLoteUe { LoteId = 1, UeId = 2, AnoEscolar = 5, DreId = 3, TotalAlunos = 10, RealizaramProva = 8 };
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(boletimLoteUe), Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<ExcluirBoletimLoteUeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
            mediator.Setup(m => m.Send(It.IsAny<InserirBoletimLoteUeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await tratarBoletimLoteUeUseCase.Executar(mensagemRabbit);
            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirBoletimLoteUeCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<InserirBoletimLoteUeCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            serviceLog.Verify(x => x.Registrar(It.IsAny<Exception>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Inserir_BoletimLoteUe()
        {
            var mensagemRabbit = new MensagemRabbit(string.Empty, Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<ExcluirBoletimLoteUeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
            mediator.Setup(m => m.Send(It.IsAny<InserirBoletimLoteUeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await tratarBoletimLoteUeUseCase.Executar(mensagemRabbit);
            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirBoletimLoteUeCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<InserirBoletimLoteUeCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            serviceLog.Verify(x => x.Registrar(It.IsAny<Exception>()), Times.Never);
        }
    }
}
