using Moq;
using SME.SERAp.Boletim.Aplicacao.Commands.BoletimResultadoProbabilidade.ExcluirPorProvaId;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Commands
{
    public class ExcluirBoletinsResultadosProbabilidadesPorProvaIdCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Excluir_Boletins_Resultados_Probabilidades_Por_ProvaId()
        {
            var provaId = 2025L;
            var retornoEsperado = 1;

            var repositorio = new Mock<IRepositorioBoletimResultadoProbabilidade>();
            repositorio
                .Setup(r => r.ExcluirBoletinsResultadosProbabilidadesPorProvaIdAsync(provaId))
                .ReturnsAsync(retornoEsperado);

            var handler = new ExcluirBoletinsResultadosProbabilidadesPorProvaIdCommandHandler(repositorio.Object);
            var command = new ExcluirBoletinsResultadosProbabilidadesPorProvaIdCommand(provaId);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(retornoEsperado, resultado);
            repositorio.Verify(r => r.ExcluirBoletinsResultadosProbabilidadesPorProvaIdAsync(provaId), Times.Once);
        }
    }
}
