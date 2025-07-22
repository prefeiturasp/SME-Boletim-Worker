using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunosBoletimResultadoProbabilidadePorTurmaId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Dominio.Teste.Queries
{
    public class ObterAlunosBoletimResultadoProbabilidadePorTurmaIdQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Lista_De_AlunoBoletimResultadoProbabilidadeDto()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimResultadoProbabilidade>();

            var turmaId = 123;
            var provaId = 456;

            var listaEsperada = new List<AlunoBoletimResultadoProbabilidadeDto>
            {
                new AlunoBoletimResultadoProbabilidadeDto { },
                new AlunoBoletimResultadoProbabilidadeDto { }
            };

            mockRepositorio
                .Setup(r => r.ObterAlunosBoletimResultadoProbabilidadePorTurmaId(turmaId, provaId))
                .ReturnsAsync(listaEsperada);

            var handler = new ObterAlunosBoletimResultadoProbabilidadePorTurmaIdQueryHandler(mockRepositorio.Object);

            var query = new ObterAlunosBoletimResultadoProbabilidadePorTurmaIdQuery(turmaId, provaId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(listaEsperada.Count, ((List<AlunoBoletimResultadoProbabilidadeDto>)resultado).Count);
            Assert.Equal(listaEsperada, resultado);

            mockRepositorio.Verify(r => r.ObterAlunosBoletimResultadoProbabilidadePorTurmaId(turmaId, provaId), Times.Once);
        }
    }
}
