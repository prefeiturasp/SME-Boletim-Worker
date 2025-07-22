using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Commands
{
    public class InserirLoteProvaCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Inserir_Lote_Prova_E_Retornar_Id()
        {
            var mockRepositorio = new Mock<IRepositorioLoteProva>();

            var loteProva = new LoteProva();
            long idRetorno = 10;

            mockRepositorio
                .Setup(r => r.IncluirAsync(loteProva))
                .ReturnsAsync(idRetorno);

            var command = new InserirLoteProvaCommand(loteProva);
            var handler = new InserirLoteProvaCommandHandler(mockRepositorio.Object);

            long resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(idRetorno, resultado);
            mockRepositorio.Verify(r => r.IncluirAsync(loteProva), Times.Once);
        }
    }
}
