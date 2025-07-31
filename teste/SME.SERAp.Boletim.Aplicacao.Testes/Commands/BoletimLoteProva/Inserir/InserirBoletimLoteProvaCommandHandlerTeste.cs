using Moq;
using System.Threading;
using System.Threading.Tasks;
using SME.SERAp.Boletim.Aplicacao;
using SME.SERAp.Boletim.Aplicacao.Commands;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Commands
{
    public class InserirBoletimLoteProvaCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Inserir_Boletim_Lote_Prova()
        {
            var boletimLoteProva = new BoletimLoteProva();
            var retornoEsperado = 789L;

            var repositorio = new Mock<IRepositorioBoletimLoteProva>();
            repositorio.Setup(r => r.IncluirAsync(boletimLoteProva)).ReturnsAsync(retornoEsperado);

            var handler = new InserirBoletimLoteProvaCommandHandler(repositorio.Object);
            var command = new InserirBoletimLoteProvaCommand(boletimLoteProva);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(retornoEsperado, resultado);
            repositorio.Verify(r => r.IncluirAsync(boletimLoteProva), Times.Once);
        }
    }
}
