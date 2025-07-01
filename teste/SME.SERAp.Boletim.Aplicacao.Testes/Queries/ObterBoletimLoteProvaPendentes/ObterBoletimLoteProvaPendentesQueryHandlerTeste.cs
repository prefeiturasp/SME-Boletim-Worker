using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimLoteProvaPendentes;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dominio.Teste.Queries
{
    public class ObterBoletimLoteProvaPendentesQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Lista_De_BoletimLoteProva()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimLoteProva>();

            var listaEsperada = new List<BoletimLoteProva>
            {
                new BoletimLoteProva(),
                new BoletimLoteProva()
            };

            mockRepositorio
                .Setup(r => r.ObterBoletimLoteProvaPendentes())
                .ReturnsAsync(listaEsperada);

            var handler = new ObterBoletimLoteProvaPendentesQueryHandler(mockRepositorio.Object);

            var query = new ObterBoletimLoteProvaPendentesQuery();

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(listaEsperada.Count, ((List<BoletimLoteProva>)resultado).Count);
            Assert.Equal(listaEsperada, resultado);

            mockRepositorio.Verify(r => r.ObterBoletimLoteProvaPendentes(), Times.Once);
        }
    }
}
