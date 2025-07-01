using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterTurmasBoletimResultadoProbabilidadePorProvaId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Dominio.Teste.Queries
{
    public class ObterTurmasBoletimResultadoProbabilidadePorProvaIdQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Lista_De_TurmaBoletimResultadoProbabilidadeDto()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimResultadoProbabilidade>();

            var provaId = 1;

            var listaEsperada = new List<TurmaBoletimResultadoProbabilidadeDto>
            {
                new TurmaBoletimResultadoProbabilidadeDto(),
                new TurmaBoletimResultadoProbabilidadeDto()
            };

            mockRepositorio
                .Setup(r => r.ObterTurmasBoletimResultadoProbabilidadePorProvaId(provaId))
                .ReturnsAsync(listaEsperada);

            var handler = new ObterTurmasBoletimResultadoProbabilidadePorProvaIdQueryHandler(mockRepositorio.Object);

            var query = new ObterTurmasBoletimResultadoProbabilidadePorProvaIdQuery(provaId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(listaEsperada.Count, ((List<TurmaBoletimResultadoProbabilidadeDto>)resultado).Count);
            Assert.Equal(listaEsperada, resultado);

            mockRepositorio.Verify(r => r.ObterTurmasBoletimResultadoProbabilidadePorProvaId(provaId), Times.Once);
        }
    }
}
