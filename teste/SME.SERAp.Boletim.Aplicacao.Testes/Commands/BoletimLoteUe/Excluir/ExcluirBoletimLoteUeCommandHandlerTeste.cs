using Moq;
using SME.SERAp.Boletim.Aplicacao.Commands.BoletimLoteUe.Excluir;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Commands
{
    public class ExcluirBoletimLoteUeCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Excluir_BoletimLoteUe()
        {
            var loteId = 123L;
            var ueId = 456L;
            var anoEscolar = 5;

            var resultadoEsperado = 1;

            var repositorioBoletimLoteUe = new Mock<IRepositorioBoletimLoteUe>();
            repositorioBoletimLoteUe
                .Setup(r => r.ExcluirBoletimLoteUe(loteId, ueId, anoEscolar))
                .ReturnsAsync(resultadoEsperado);

            var handler = new ExcluirBoletimLoteUeCommandHandler(repositorioBoletimLoteUe.Object);
            var command = new ExcluirBoletimLoteUeCommand(loteId, ueId, anoEscolar);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(resultadoEsperado, resultado);
            repositorioBoletimLoteUe.Verify(r => r.ExcluirBoletimLoteUe(loteId, ueId, anoEscolar), Times.Once);
        }
    }
}
