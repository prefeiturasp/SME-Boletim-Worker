using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries
{
    public class ObterProvasTaiPorLoteIdQueryHandlerTeste
    {
        private readonly Mock<IRepositorioLoteProva> repositorioLoteProva;
        private readonly ObterProvasTaiPorLoteIdQueryHandler handler;

        public ObterProvasTaiPorLoteIdQueryHandlerTeste()
        {
            repositorioLoteProva = new Mock<IRepositorioLoteProva>();
            handler = new ObterProvasTaiPorLoteIdQueryHandler(repositorioLoteProva.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Provas()
        {
            var loteId = 1;
            var query = new ObterProvasTaiPorLoteIdQuery(loteId);
            var provas = new List<ProvaDto>
            {
                new ProvaDto(),
                new ProvaDto()
            };

            repositorioLoteProva
                .Setup(r => r.ObterProvasTaiAnoPorLoteId(loteId))
                .ReturnsAsync(provas);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(provas, resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Repositorio_Lancar()
        {
            var loteId = 1;
            var query = new ObterProvasTaiPorLoteIdQuery(loteId);

            repositorioLoteProva
                .Setup(r => r.ObterProvasTaiAnoPorLoteId(loteId))
                .ThrowsAsync(new Exception("Erro inesperado"));

            var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));

            Assert.Equal("Erro inesperado", exception.Message);
        }
    }
}
