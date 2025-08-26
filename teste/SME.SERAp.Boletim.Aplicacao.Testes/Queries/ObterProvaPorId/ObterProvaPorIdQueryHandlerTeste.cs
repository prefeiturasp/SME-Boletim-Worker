using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaPorId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries
{
    public class ObterProvaPorIdQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Prova_Pelo_Id()
        {
            var mockRepositorio = new Mock<IRepositorioProva>();

            var provaEsperada = new Prova { Id = 1 };

            mockRepositorio
                .Setup(r => r.ObterPorIdAsync(provaEsperada.Id))
                .ReturnsAsync(provaEsperada);

            var handler = new ObterProvaPorIdQueryHandler(mockRepositorio.Object);

            var query = new ObterProvaPorIdQuery(provaEsperada.Id);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(provaEsperada.Id, resultado.Id);
            Assert.Equal(provaEsperada, resultado);

            mockRepositorio.Verify(r => r.ObterPorIdAsync(provaEsperada.Id), Times.Once);
        }
    }
}
