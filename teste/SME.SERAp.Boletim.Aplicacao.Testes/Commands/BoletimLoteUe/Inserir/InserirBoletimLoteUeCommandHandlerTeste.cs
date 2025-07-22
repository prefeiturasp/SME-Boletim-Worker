using Moq;
using SME.SERAp.Boletim.Aplicacao.Commands;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Commands
{
    public class InserirBoletimLoteUeCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Inserir_BoletimLoteUe()
        {
            var boletimLoteUe = new BoletimLoteUe();
            var retornoEsperado = 456L;

            var repositorio = new Mock<IRepositorioBoletimLoteUe>();
            repositorio.Setup(r => r.IncluirAsync(boletimLoteUe)).ReturnsAsync(retornoEsperado);

            var handler = new InserirBoletimLoteUeCommandHandler(repositorio.Object);
            var command = new InserirBoletimLoteUeCommand(boletimLoteUe);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(retornoEsperado, resultado);
            repositorio.Verify(r => r.IncluirAsync(boletimLoteUe), Times.Once);
        }
    }
}
