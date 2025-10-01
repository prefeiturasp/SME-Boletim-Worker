using Moq;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Commands
{
    public class InserirBoletimEscolarCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Inserir_Boletim_Escolar()
        {
            var boletim = new BoletimEscolar();
            var retornoEsperado = 456L;

            var repositorio = new Mock<IRepositorioBoletimEscolar>();
            repositorio.Setup(r => r.IncluirAsync(boletim)).ReturnsAsync(retornoEsperado);

            var handler = new InserirBoletimEscolarCommandHandler(repositorio.Object);
            var command = new InserirBoletimEscolarCommand(boletim);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(retornoEsperado, resultado);
            repositorio.Verify(r => r.IncluirAsync(boletim), Times.Once);
        }
    }
}
