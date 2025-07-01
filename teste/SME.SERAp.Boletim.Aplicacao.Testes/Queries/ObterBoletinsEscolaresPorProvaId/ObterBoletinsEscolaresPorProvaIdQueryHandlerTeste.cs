using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresPorProvaId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dominio.Teste.Queries
{
    public class ObterBoletinsEscolaresPorProvaIdQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Lista_De_BoletimEscolar()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimEscolar>();

            var provaId = 1;

            var listaEsperada = new List<BoletimEscolar>
            {
                new BoletimEscolar(),
                new BoletimEscolar()
            };

            mockRepositorio
                .Setup(r => r.ObterBoletinsEscolaresPorProvaId(provaId))
                .ReturnsAsync(listaEsperada);

            var handler = new ObterBoletinsEscolaresPorProvaIdQueryHandler(mockRepositorio.Object);

            var query = new ObterBoletinsEscolaresPorProvaIdQuery(provaId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(listaEsperada.Count, ((List<BoletimEscolar>)resultado).Count);
            Assert.Equal(listaEsperada, resultado);

            mockRepositorio.Verify(r => r.ObterBoletinsEscolaresPorProvaId(provaId), Times.Once);
        }
    }
}
