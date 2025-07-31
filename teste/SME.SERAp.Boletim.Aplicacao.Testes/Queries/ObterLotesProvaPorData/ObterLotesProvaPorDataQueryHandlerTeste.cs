using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterLotesProvaPorData;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dominio.Teste.Queries
{
    public class ObterLotesProvaPorDataQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Lista_De_LoteProva()
        {
            var mockRepositorio = new Mock<IRepositorioLoteProva>();

            var inicio = new DateTime(2024, 1, 1);
            var fim = new DateTime(2024, 12, 31);
            var formatoTai = false;

            var listaEsperada = new List<LoteProva>
            {
                new LoteProva(),
                new LoteProva()
            };

            mockRepositorio
                .Setup(r => r.ObterLotesProvaPorData(inicio, fim, formatoTai))
                .ReturnsAsync(listaEsperada);

            var handler = new ObterLotesProvaPorDataQueryHandler(mockRepositorio.Object);

            var query = new ObterLotesProvaPorDataQuery(inicio, fim, formatoTai);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(listaEsperada.Count, ((List<LoteProva>)resultado).Count);
            Assert.Equal(listaEsperada, resultado);

            mockRepositorio.Verify(r => r.ObterLotesProvaPorData(inicio, fim, formatoTai), Times.Once);
        }
    }
}
