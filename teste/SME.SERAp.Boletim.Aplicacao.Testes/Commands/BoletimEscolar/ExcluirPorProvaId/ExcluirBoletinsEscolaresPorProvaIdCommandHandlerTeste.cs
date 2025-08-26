using Moq;
using System.Threading;
using System.Threading.Tasks;
using SME.SERAp.Boletim.Aplicacao;
using SME.SERAp.Boletim.Aplicacao.Commands;
using SME.SERAp.Boletim.Dados.Interfaces;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Commands
{
    public class ExcluirBoletinsEscolaresPorProvaIdCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Excluir_Boletins_Escolares_Por_ProvaId()
        {
            var provaId = 123;
            var resultadoEsperado = 1;

            var repositorioBoletimEscolar = new Mock<IRepositorioBoletimEscolar>();
            repositorioBoletimEscolar
                .Setup(r => r.ExcluirBoletinsEscolaresPorProvaIdAsync(provaId))
                .ReturnsAsync(resultadoEsperado);

            var handler = new ExcluirBoletinsEscolaresPorProvaIdCommandHandler(repositorioBoletimEscolar.Object);
            var command = new ExcluirBoletinsEscolaresPorProvaIdCommand(provaId);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(resultadoEsperado, resultado);
            repositorioBoletimEscolar.Verify(r => r.ExcluirBoletinsEscolaresPorProvaIdAsync(provaId), Times.Once);
        }
    }
}
