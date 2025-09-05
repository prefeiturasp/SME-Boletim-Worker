using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaIdAnoEscolar;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries
{
    public class ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Lista_De_NivelProficiencia()
        {
            var mockRepositorio = new Mock<IRepositorioNivelProficiencia>();

            var disciplinaId = 1;
            var anoEscolar = 2024;

            var listaEsperada = new List<NivelProficiencia>
            {
                new NivelProficiencia(),
                new NivelProficiencia()
            };

            mockRepositorio
                .Setup(r => r.ObterNiveisProficienciaPorDisciplinaIdAnoEscolar(disciplinaId, anoEscolar))
                .ReturnsAsync(listaEsperada);

            var handler = new ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQueryHandler(mockRepositorio.Object);

            var query = new ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQuery(disciplinaId, anoEscolar);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(listaEsperada.Count, ((List<NivelProficiencia>)resultado).Count);
            Assert.Equal(listaEsperada, resultado);

            mockRepositorio.Verify(r => r.ObterNiveisProficienciaPorDisciplinaIdAnoEscolar(disciplinaId, anoEscolar), Times.Once);
        }
    }
}
