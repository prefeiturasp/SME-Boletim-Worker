using Moq;
using Nest;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries
{
    public class ObterUesTotalAlunosPorLoteIdQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimLoteUe> repositorioBoletimLoteUe;
        private readonly ObterUesTotalAlunosPorLoteIdQueryHandler queryHandler;
        public ObterUesTotalAlunosPorLoteIdQueryHandlerTeste()
        {
            repositorioBoletimLoteUe = new Mock<IRepositorioBoletimLoteUe>();
            queryHandler = new ObterUesTotalAlunosPorLoteIdQueryHandler(repositorioBoletimLoteUe.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Ues_Total_Alunos_Por_LoteId()
        {
            var loteId = 1;
            var uesEsperadas = new List<BoletimLoteUe>
            {
                new BoletimLoteUe(1,2,loteId,5,6,7),
                new BoletimLoteUe(8,9,loteId,11,12,13)
            };

            repositorioBoletimLoteUe
                .Setup(r => r.ObterUesTotalAlunosPorLoteId(loteId))
                .ReturnsAsync(uesEsperadas);

            var query = new ObterUesTotalAlunosPorLoteIdQuery(loteId);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(uesEsperadas.Count, resultado.Count());
            Assert.Equal(resultado, resultado.ToList());
            Assert.Contains(uesEsperadas, x => x.LoteId == loteId);
            repositorioBoletimLoteUe.Verify(r => r.ObterUesTotalAlunosPorLoteId(loteId), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Ues_Total_Alunos_Por_LoteId()
        {
            var loteId = 2;
            repositorioBoletimLoteUe
                .Setup(r => r.ObterUesTotalAlunosPorLoteId(loteId))
                .ReturnsAsync(new List<BoletimLoteUe>());

            var query = new ObterUesTotalAlunosPorLoteIdQuery(loteId);

            var resultado = await queryHandler.Handle(query, CancellationToken.None);
            Assert.Empty(resultado);
            repositorioBoletimLoteUe.Verify(r => r.ObterUesTotalAlunosPorLoteId(loteId), Times.Once);
        }
    }
}
