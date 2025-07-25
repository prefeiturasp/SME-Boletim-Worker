using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Commands.BoletimLoteUe.Excluir;
using SME.SERAp.Boletim.Dados.Interfaces;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Commands
{
    public class ExcluirBoletimLoteUeCommandHandlerTeste
    {
        [Fact(DisplayName = "Deve excluir BoletimLoteUe com sucesso")]
        public async Task Deve_Excluir_BoletimLoteUe()
        {
            // Arrange
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

            // Act
            var resultado = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(resultadoEsperado, resultado);
            repositorioBoletimLoteUe.Verify(r => r.ExcluirBoletimLoteUe(loteId, ueId, anoEscolar), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar 0 se nenhum registro for excluído")]
        public async Task Deve_Retornar_Zero_Se_Nenhum_Registro_Excluido()
        {
            // Arrange
            var repositorioMock = new Mock<IRepositorioBoletimLoteUe>();
            repositorioMock
                .Setup(r => r.ExcluirBoletimLoteUe(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<int>()))
                .ReturnsAsync(0);

            var handler = new ExcluirBoletimLoteUeCommandHandler(repositorioMock.Object);
            var command = new ExcluirBoletimLoteUeCommand(1, 2, 3);

            // Act
            var resultado = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(0, resultado);
            repositorioMock.Verify(r => r.ExcluirBoletimLoteUe(1, 2, 3), Times.Once);
        }

        [Fact(DisplayName = "Deve propagar exceção do repositório")]
        public async Task Deve_Lancar_Excecao_Se_Repositorio_Falhar()
        {
            // Arrange
            var repositorioMock = new Mock<IRepositorioBoletimLoteUe>();
            repositorioMock
                .Setup(r => r.ExcluirBoletimLoteUe(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro no repositório"));

            var handler = new ExcluirBoletimLoteUeCommandHandler(repositorioMock.Object);
            var command = new ExcluirBoletimLoteUeCommand(1, 2, 3);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            repositorioMock.Verify(r => r.ExcluirBoletimLoteUe(1, 2, 3), Times.Once);
        }
    }
}