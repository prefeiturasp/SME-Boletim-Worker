using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao;
using SME.SERAp.Boletim.Dados.Interfaces;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Commands
{
    public class DesativarTodosLotesProvaAtivosCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Desativar_Todos_Lotes_Ativos_E_Retornar_Int()
        {
            var mockRepositorio = new Mock<IRepositorioLoteProva>();

            int retornoEsperado = 1;

            mockRepositorio
                .Setup(r => r.DesativarTodosLotesProvaAtivos())
                .ReturnsAsync(retornoEsperado);

            var command = new DesativarTodosLotesProvaAtivosCommand();
            var handler = new DesativarTodosLotesProvaAtivosCommandHandler(mockRepositorio.Object);

            int resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(retornoEsperado, resultado);
            mockRepositorio.Verify(r => r.DesativarTodosLotesProvaAtivos(), Times.Once);
        }
    }
}
