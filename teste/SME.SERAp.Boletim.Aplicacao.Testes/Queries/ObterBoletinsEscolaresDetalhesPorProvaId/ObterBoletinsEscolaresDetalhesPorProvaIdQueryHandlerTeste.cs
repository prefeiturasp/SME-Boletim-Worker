using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresDetalhesPorProvaId;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries
{
    public class ObterBoletinsEscolaresDetalhesPorProvaIdQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Lista_De_BoletimEscolarDetalhesDto()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimProvaAluno>();

            var provaId = 1;

            var listaEsperada = new List<BoletimEscolarDetalhesDto>
            {
                new BoletimEscolarDetalhesDto(),
                new BoletimEscolarDetalhesDto()
            };

            mockRepositorio
                .Setup(r => r.ObterBoletinsEscolaresDetalhesPorProvaId(provaId))
                .ReturnsAsync(listaEsperada);

            var handler = new ObterBoletinsEscolaresDetalhesPorProvaIdQueryHandler(mockRepositorio.Object);

            var query = new ObterBoletinsEscolaresDetalhesPorProvaIdQuery(provaId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(listaEsperada.Count, ((List<BoletimEscolarDetalhesDto>)resultado).Count);
            Assert.Equal(listaEsperada, resultado);

            mockRepositorio.Verify(r => r.ObterBoletinsEscolaresDetalhesPorProvaId(provaId), Times.Once);
        }
    }
}
