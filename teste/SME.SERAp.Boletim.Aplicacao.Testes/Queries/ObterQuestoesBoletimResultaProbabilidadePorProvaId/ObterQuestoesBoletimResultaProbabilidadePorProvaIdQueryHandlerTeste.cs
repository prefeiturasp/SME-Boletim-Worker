using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterQuestoesBoletimResultaProbabilidadePorProvaId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Dominio.Teste.Queries
{
    public class ObterQuestoesBoletimResultaProbabilidadePorProvaIdQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Lista_De_QuestaoProvaDto()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimResultadoProbabilidade>();

            var provaId = 1;

            var listaEsperada = new List<QuestaoProvaDto>
            {
                new QuestaoProvaDto(),
                new QuestaoProvaDto()
            };

            mockRepositorio
                .Setup(r => r.ObterQuestoesBoletimResultaProbabilidadePorProvaId(provaId))
                .ReturnsAsync(listaEsperada);

            var handler = new ObterQuestoesBoletimResultaProbabilidadePorProvaIdQueryHandler(mockRepositorio.Object);

            var query = new ObterQuestoesBoletimResultaProbabilidadePorProvaIdQuery(provaId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(listaEsperada.Count, ((List<QuestaoProvaDto>)resultado).Count);
            Assert.Equal(listaEsperada, resultado);

            mockRepositorio.Verify(r => r.ObterQuestoesBoletimResultaProbabilidadePorProvaId(provaId), Times.Once);
        }
    }
}
