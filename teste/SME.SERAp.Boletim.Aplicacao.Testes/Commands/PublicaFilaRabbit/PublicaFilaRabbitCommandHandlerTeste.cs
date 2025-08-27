using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Dominio.Enums;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Commands
{
    public class PublicaFilaRabbitCommandHandlerTeste
    {
        private readonly Mock<IChannel> channel;
        private readonly Mock<IServicoLog> servicoLog;
        private readonly PublicaFilaRabbitCommandHandler handler;

        public PublicaFilaRabbitCommandHandlerTeste()
        {
            channel = new Mock<IChannel>();
            servicoLog = new Mock<IServicoLog>();
            handler = new PublicaFilaRabbitCommandHandler(channel.Object, servicoLog.Object);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Channel_Nulo_No_Construtor()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new PublicaFilaRabbitCommandHandler(null!, servicoLog.Object));
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_ServicoLog_Nulo_No_Construtor()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new PublicaFilaRabbitCommandHandler(channel.Object, null!));
        }

        [Fact]
        public async Task Deve_Retornar_True_Quando_Publicacao_Bem_Sucedida()
        {
            var command = new PublicaFilaRabbitCommand("fila_teste");
            channel
                .Setup(c => c.BasicPublishAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<BasicProperties>(),
                    It.IsAny<ReadOnlyMemory<byte>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask());

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.True(resultado);
            channel
                .Verify(c => c.BasicPublishAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<BasicProperties>(),
                    It.IsAny<ReadOnlyMemory<byte>>(),
                    It.IsAny<CancellationToken>()),
                    Times.AtLeastOnce);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Publicacao_Lancar_Excecao()
        {
            var command = new PublicaFilaRabbitCommand("fila_teste");

            channel
                .Setup(c => c.BasicPublishAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<BasicProperties>(),
                    It.IsAny<ReadOnlyMemory<byte>>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Falha no RabbitMQ"));

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.False(resultado);
            servicoLog.Verify(s => s.Registrar(It.IsAny<LogNivel>(),
                                                   It.IsAny<string>(),
                                                   It.IsAny<string>(),
                                                   It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Registrar_LogCritico_No_Retry()
        {
            var command = new PublicaFilaRabbitCommand("fila_teste");

            int tentativas = 0;
            channel
                .Setup(c => c.BasicPublishAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<BasicProperties>(),
                    It.IsAny<ReadOnlyMemory<byte>>(),
                    It.IsAny<CancellationToken>()))
                .Callback(() =>
                {
                    tentativas++;
                    throw new Exception("Erro simulado");
                });

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.False(resultado);
            Assert.True(tentativas >= 1);
            servicoLog.Verify(s => s.Registrar(It.Is<string>(nivel => nivel == LogNivel.Critico.ToString()),
                                                   It.IsAny<Exception>()), Times.AtLeastOnce);
        }
    }
}
