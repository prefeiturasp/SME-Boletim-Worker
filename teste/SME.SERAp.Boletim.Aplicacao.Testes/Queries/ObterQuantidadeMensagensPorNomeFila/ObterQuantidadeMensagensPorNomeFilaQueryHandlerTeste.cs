using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterQuantidadeMensagensPorNomeFila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries
{
    public class ObterQuantidadeMensagensPorNomeFilaQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Quantidade_De_Mensagens()
        {
            var nomeFila = "filaTeste";
            var quantidadeEsperada = 42;

            var mockServicoLog = new Mock<IServicoLog>();

            var mockChannel = new Mock<IChannel>();
            mockChannel
                .Setup(c => c.MessageCountAsync(nomeFila, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult((uint)quantidadeEsperada));

            mockChannel
                .Setup(c => c.DisposeAsync())
                .Returns(ValueTask.CompletedTask);

            var mockConnection = new Mock<IConnection>();
            mockConnection
                .Setup(c => c.CreateChannelAsync(null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockChannel.Object);

            var handler = new ObterQuantidadeMensagensPorNomeFilaQueryHandler(mockServicoLog.Object, mockConnection.Object);

            var query = new ObterQuantidadeMensagensPorNomeFilaQuery(nomeFila);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(quantidadeEsperada, resultado);

            mockConnection.Verify(c => c.CreateChannelAsync(null, It.IsAny<CancellationToken>()), Times.Once);
            mockChannel.Verify(c => c.MessageCountAsync(nomeFila, It.IsAny<CancellationToken>()), Times.Once);
            mockChannel.Verify(c => c.Dispose(), Times.Once);

        }

        [Fact]
        public async Task Deve_Registrar_Erro_Quando_Excecao()
        {
            var nomeFila = "filaTeste";

            var mockServicoLog = new Mock<IServicoLog>();

            var mockConnection = new Mock<IConnection>();
            mockConnection
                .Setup(c => c.CreateChannelAsync(null, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro no canal"));

            var handler = new ObterQuantidadeMensagensPorNomeFilaQueryHandler(mockServicoLog.Object, mockConnection.Object);

            var query = new ObterQuantidadeMensagensPorNomeFilaQuery(nomeFila);

            await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));

            mockServicoLog.Verify(s => s.Registrar(It.IsAny<Exception>()), Times.Once);
        }
    }
}
