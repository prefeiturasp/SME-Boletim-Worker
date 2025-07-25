using Moq;
using SME.SERAp.Boletim.Aplicacao.Commands;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Commands
{
    public class InserirBoletimLoteUeCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Inserir_BoletimLoteUe()
        {
            // Arrange
            var boletimLoteUe = new BoletimLoteUe();
            var retornoEsperado = 456L;

            var repositorio = new Mock<IRepositorioBoletimLoteUe>();
            repositorio
                .Setup(r => r.IncluirAsync(boletimLoteUe))
                .ReturnsAsync(retornoEsperado);

            var handler = new InserirBoletimLoteUeCommandHandler(repositorio.Object);
            var command = new InserirBoletimLoteUeCommand(boletimLoteUe);

            // Act
            var resultado = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(retornoEsperado, resultado);
            repositorio.Verify(r => r.IncluirAsync(boletimLoteUe), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Ocorre_Erro_Ao_Inserir()
        {
            // Arrange
            var boletimLoteUe = new BoletimLoteUe();

            var repositorio = new Mock<IRepositorioBoletimLoteUe>();
            repositorio
                .Setup(r => r.IncluirAsync(It.IsAny<BoletimLoteUe>()))
                .ThrowsAsync(new Exception("Erro ao inserir"));

            var handler = new InserirBoletimLoteUeCommandHandler(repositorio.Object);
            var command = new InserirBoletimLoteUeCommand(boletimLoteUe);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            repositorio.Verify(r => r.IncluirAsync(boletimLoteUe), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Chamar_Insercao_Quando_BoletimLoteUe_For_Nulo()
        {
            // Arrange
            var repositorio = new Mock<IRepositorioBoletimLoteUe>();

            var handler = new InserirBoletimLoteUeCommandHandler(repositorio.Object);
            var command = new InserirBoletimLoteUeCommand(null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));
            repositorio.Verify(r => r.IncluirAsync(It.IsAny<BoletimLoteUe>()), Times.Never);
        }
    }
}