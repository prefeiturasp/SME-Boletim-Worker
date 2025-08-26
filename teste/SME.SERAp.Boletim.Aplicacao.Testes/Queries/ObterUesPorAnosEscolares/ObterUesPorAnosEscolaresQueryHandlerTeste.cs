using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries
{
    public class ObterUesPorAnosEscolaresQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimLoteUe> repositorioBoletimLoteUe;
        private readonly ObterUesPorAnosEscolaresQueryHandler handler;

        public ObterUesPorAnosEscolaresQueryHandlerTeste()
        {
            repositorioBoletimLoteUe = new Mock<IRepositorioBoletimLoteUe>();
            handler = new ObterUesPorAnosEscolaresQueryHandler(repositorioBoletimLoteUe.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Ues()
        {
            var anosEscolares = new List<string> { "1", "2", "3" };
            var anoLetivo = 2025;
            var query = new ObterUesPorAnosEscolaresQuery(anosEscolares, anoLetivo);
            var ues = new List<UeDto>
            {
                new UeDto(),
                new UeDto()
            };

            repositorioBoletimLoteUe
                .Setup(r => r.ObterUesPorAnosEscolares(anosEscolares, anoLetivo))
                .ReturnsAsync(ues);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(ues, resultado);
        }

        [Fact]
        public async Task Deve_LancarExcecao_QuandoRepositorioLancar()
        {
            var anosEscolares = new List<string> { "1", "2", "3" };
            var anoLetivo = 2025;
            var query = new ObterUesPorAnosEscolaresQuery(anosEscolares, anoLetivo);

            repositorioBoletimLoteUe
                .Setup(r => r.ObterUesPorAnosEscolares(anosEscolares, anoLetivo))
                .ThrowsAsync(new Exception("Erro inesperado"));

            var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));

            Assert.Equal("Erro inesperado", exception.Message);
        }
    }
}
