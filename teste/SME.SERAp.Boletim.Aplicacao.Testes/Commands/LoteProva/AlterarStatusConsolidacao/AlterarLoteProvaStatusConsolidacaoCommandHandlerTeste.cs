using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Commands.LoteProva.AlterarStatusConsolidacao;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Enums;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Commands
{
    public class AlterarLoteProvaStatusConsolidacaoCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Alterar_Status_Consolidacao_E_Retornar_Int()
        {
            var mockRepositorio = new Mock<IRepositorioLoteProva>();

            int idLoteProva = 123;
            LoteStatusConsolidacao statusConsolidacao = LoteStatusConsolidacao.Consolidado;
            int retornoEsperado = 1;

            mockRepositorio
                .Setup(r => r.AlterarStatusConsolidacao(idLoteProva, statusConsolidacao))
                .ReturnsAsync(retornoEsperado);

            var command = new AlterarLoteProvaStatusConsolidacaoCommand(idLoteProva, statusConsolidacao);
            var handler = new AlterarLoteProvaStatusConsolidacaoCommandHandler(mockRepositorio.Object);

            int resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(retornoEsperado, resultado);
            mockRepositorio.Verify(r => r.AlterarStatusConsolidacao(idLoteProva, statusConsolidacao), Times.Once);
        }
    }
}
