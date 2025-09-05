using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries
{
    public class ObterAnoProvaQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Obter_Ano_Prova()
        {
            var provaId = 1;
            var anoEsperado = 2023;

            var repositorioProvaMock = new Mock<IRepositorioProva>();
            repositorioProvaMock.Setup(r => r.ObterAnoProva(provaId)).ReturnsAsync(anoEsperado);

            var handler = new ObterAnoProvaQueryHandler(repositorioProvaMock.Object);
            var query = new ObterAnoProvaQuery(provaId);
            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(anoEsperado, resultado);
        }

        [Fact]
        public async Task Deve_Obter_Ano_Prova_Nulo()
        {
            var provaId = 1;

            var repositorioProvaMock = new Mock<IRepositorioProva>();
            repositorioProvaMock.Setup(r => r.ObterAnoProva(provaId)).ReturnsAsync((int?)null);

            var handler = new ObterAnoProvaQueryHandler(repositorioProvaMock.Object);
            var query = new ObterAnoProvaQuery(provaId);
            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Null(resultado);
        }
    }
}
