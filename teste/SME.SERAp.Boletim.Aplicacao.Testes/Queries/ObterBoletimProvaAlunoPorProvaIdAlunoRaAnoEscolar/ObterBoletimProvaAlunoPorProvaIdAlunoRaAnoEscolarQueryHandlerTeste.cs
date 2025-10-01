using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolar;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries
{
    public class ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Lista_De_BoletimProvaAluno()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimProvaAluno>();

            var provaId = 1;
            long alunoRa = 123456;
            var anoEscolar = 2024;

            var listaEsperada = new List<BoletimProvaAluno>
            {
                new BoletimProvaAluno(),
                new BoletimProvaAluno()
            };

            mockRepositorio
                .Setup(r => r.ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolar(provaId, alunoRa, anoEscolar))
                .ReturnsAsync(listaEsperada);

            var handler = new ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQueryHandler(mockRepositorio.Object);

            var query = new ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQuery(provaId, alunoRa, anoEscolar);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(listaEsperada.Count, ((List<BoletimProvaAluno>)resultado).Count);
            Assert.Equal(listaEsperada, resultado);

            mockRepositorio.Verify(r => r.ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolar(provaId, alunoRa, anoEscolar), Times.Once);
        }
    }
}
