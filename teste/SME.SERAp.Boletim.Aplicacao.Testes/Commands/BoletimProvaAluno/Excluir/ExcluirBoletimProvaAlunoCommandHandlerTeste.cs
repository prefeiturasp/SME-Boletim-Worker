using Moq;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Commands
{
    public class ExcluirBoletimProvaAlunoCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Excluir_Boletim_Prova_Aluno_Por_Id()
        {
            var id = 101;
            var retornoEsperado = 1;

            var repositorio = new Mock<IRepositorioBoletimProvaAluno>();
            repositorio.Setup(r => r.ExcluirPorIdAsync(id)).ReturnsAsync(retornoEsperado);

            var handler = new ExcluirBoletimProvaAlunoCommandHandler(repositorio.Object);
            var command = new ExcluirBoletimProvaAlunoCommand(id);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(retornoEsperado, resultado);
            repositorio.Verify(r => r.ExcluirPorIdAsync(id), Times.Once);
        }
    }
}
