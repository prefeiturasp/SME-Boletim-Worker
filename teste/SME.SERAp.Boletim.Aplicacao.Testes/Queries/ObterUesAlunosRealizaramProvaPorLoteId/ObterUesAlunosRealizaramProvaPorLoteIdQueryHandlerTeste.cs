using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries
{
    public class ObterUesAlunosRealizaramProvaPorLoteIdQueryHandlerTeste
    {
        private readonly Mock<IRepositorioBoletimLoteUe> repositorioBoletimLoteUe;
        private readonly ObterUesAlunosRealizaramProvaPorLoteIdQueryHandler queryHandler;
        public ObterUesAlunosRealizaramProvaPorLoteIdQueryHandlerTeste()
        {
            repositorioBoletimLoteUe = new Mock<IRepositorioBoletimLoteUe>();
            queryHandler = new ObterUesAlunosRealizaramProvaPorLoteIdQueryHandler(repositorioBoletimLoteUe.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Ues_Alunos_Realizaram_Prova_Por_LoteId()
        {
            var loteId = 1;
            var uesEsperadas = new List<BoletimLoteUeRealizaramProvaDto>
            {
                new BoletimLoteUeRealizaramProvaDto { DreId = 1, UeId = 2, LoteId = loteId, AnoEscolar = 3, RealizaramProva = 4},
                new BoletimLoteUeRealizaramProvaDto { DreId = 5, UeId = 6, LoteId = loteId, AnoEscolar = 7, RealizaramProva = 8}
            };

            repositorioBoletimLoteUe
                .Setup(r => r.ObterUesAlunosRealizaramProvaPorLoteId(loteId))
                .ReturnsAsync(uesEsperadas);

            var query = new ObterUesAlunosRealizaramProvaPorLoteIdQuery(loteId);
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(uesEsperadas.Count, resultado.Count());
            Assert.Equal(resultado, resultado.ToList());
            Assert.Contains(uesEsperadas, x => x.LoteId == loteId);
            repositorioBoletimLoteUe.Verify(r => r.ObterUesAlunosRealizaramProvaPorLoteId(loteId), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Ues_Alunos_Realizaram_Prova_Por_LoteId()
        {
            var loteId = 1;

            repositorioBoletimLoteUe
                .Setup(r => r.ObterUesAlunosRealizaramProvaPorLoteId(loteId))
                .ReturnsAsync(new List<BoletimLoteUeRealizaramProvaDto>());

            var query = new ObterUesAlunosRealizaramProvaPorLoteIdQuery(loteId);
            var resultado = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Empty(resultado);
            repositorioBoletimLoteUe.Verify(r => r.ObterUesAlunosRealizaramProvaPorLoteId(loteId), Times.Once);
        }
    }
}
