using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvasFinalizadasPorData;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries
{
    public class ObterProvasFinalizadasPorDataQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Lista_De_ProvaDto()
        {
            var mockRepositorio = new Mock<IRepositorioProva>();

            var data = new DateTime(2024, 6, 30);

            var listaEsperada = new List<ProvaDto>
            {
                new ProvaDto(),
                new ProvaDto()
            };

            mockRepositorio
                .Setup(r => r.ObterProvasFinalizadasPorData(data))
                .ReturnsAsync(listaEsperada);

            var handler = new ObterProvasFinalizadasPorDataQueryHandler(mockRepositorio.Object);

            var query = new ObterProvasFinalizadasPorDataQuery(data);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(listaEsperada.Count, ((List<ProvaDto>)resultado).Count);
            Assert.Equal(listaEsperada, resultado);

            mockRepositorio.Verify(r => r.ObterProvasFinalizadasPorData(data), Times.Once);
        }
    }
}
