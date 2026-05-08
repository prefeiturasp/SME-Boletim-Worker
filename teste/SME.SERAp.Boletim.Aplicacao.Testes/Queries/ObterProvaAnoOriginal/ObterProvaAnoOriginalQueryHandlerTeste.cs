using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaAnoOriginal;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries.ObterProvaAnoOriginal
{
    public class ObterProvaAnoOriginalQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Ano_Original_Da_Prova()
        {
            var mockRepositorio = new Mock<IRepositorioProva>();

            var provaId = 123L;
            var anoOriginalEsperado = "2024";

            mockRepositorio
                .Setup(r => r.ObterProvaAnoOriginal(provaId))
                .ReturnsAsync(anoOriginalEsperado);

            var handler = new ObterProvaAnoOriginalQueryHandler(mockRepositorio.Object);
            var query = new ObterProvaAnoOriginalQuery(provaId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(anoOriginalEsperado, resultado);

            mockRepositorio.Verify(r => r.ObterProvaAnoOriginal(provaId), Times.Once);
        }

        [Fact]
        public async Task Deve_Chamar_Repositorio_Com_ProvaId_Correto()
        {
            var mockRepositorio = new Mock<IRepositorioProva>();

            var provaId = 456L;
            var anoOriginal = "2023";

            mockRepositorio
                .Setup(r => r.ObterProvaAnoOriginal(It.IsAny<long>()))
                .ReturnsAsync(anoOriginal);

            var handler = new ObterProvaAnoOriginalQueryHandler(mockRepositorio.Object);
            var query = new ObterProvaAnoOriginalQuery(provaId);

            await handler.Handle(query, CancellationToken.None);

            mockRepositorio.Verify(r => r.ObterProvaAnoOriginal(provaId), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_String_Vazia_Quando_Prova_Nao_Encontrada()
        {
            var mockRepositorio = new Mock<IRepositorioProva>();

            var provaId = 789L;
            var anoOriginalEsperado = string.Empty;

            mockRepositorio
                .Setup(r => r.ObterProvaAnoOriginal(provaId))
                .ReturnsAsync(anoOriginalEsperado);

            var handler = new ObterProvaAnoOriginalQueryHandler(mockRepositorio.Object);
            var query = new ObterProvaAnoOriginalQuery(provaId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(anoOriginalEsperado, resultado);
            Assert.Empty(resultado);

            mockRepositorio.Verify(r => r.ObterProvaAnoOriginal(provaId), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Null_Quando_Repositorio_Retorna_Null()
        {
            var mockRepositorio = new Mock<IRepositorioProva>();

            var provaId = 999L;

            mockRepositorio
                .Setup(r => r.ObterProvaAnoOriginal(provaId))
                .ReturnsAsync((string)null);

            var handler = new ObterProvaAnoOriginalQueryHandler(mockRepositorio.Object);
            var query = new ObterProvaAnoOriginalQuery(provaId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Null(resultado);

            mockRepositorio.Verify(r => r.ObterProvaAnoOriginal(provaId), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Chamar_Repositorio_Mais_De_Uma_Vez()
        {
            var mockRepositorio = new Mock<IRepositorioProva>();

            var provaId = 111L;
            var anoOriginal = "2025";

            mockRepositorio
                .Setup(r => r.ObterProvaAnoOriginal(provaId))
                .ReturnsAsync(anoOriginal);

            var handler = new ObterProvaAnoOriginalQueryHandler(mockRepositorio.Object);
            var query = new ObterProvaAnoOriginalQuery(provaId);

            await handler.Handle(query, CancellationToken.None);

            mockRepositorio.Verify(r => r.ObterProvaAnoOriginal(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Propagar_Excecao_Do_Repositorio()
        {
            var mockRepositorio = new Mock<IRepositorioProva>();

            var provaId = 222L;

            mockRepositorio
                .Setup(r => r.ObterProvaAnoOriginal(provaId))
                .ThrowsAsync(new InvalidOperationException("Erro ao acessar repositório"));

            var handler = new ObterProvaAnoOriginalQueryHandler(mockRepositorio.Object);
            var query = new ObterProvaAnoOriginalQuery(provaId);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(query, CancellationToken.None));

            mockRepositorio.Verify(r => r.ObterProvaAnoOriginal(provaId), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Ano_Diferente_Para_ProvaIds_Diferentes()
        {
            var mockRepositorio = new Mock<IRepositorioProva>();

            var provaId1 = 333L;
            var provaId2 = 444L;
            var anoOriginal1 = "2022";
            var anoOriginal2 = "2021";

            mockRepositorio
                .Setup(r => r.ObterProvaAnoOriginal(provaId1))
                .ReturnsAsync(anoOriginal1);

            mockRepositorio
                .Setup(r => r.ObterProvaAnoOriginal(provaId2))
                .ReturnsAsync(anoOriginal2);

            var handler = new ObterProvaAnoOriginalQueryHandler(mockRepositorio.Object);
            var query1 = new ObterProvaAnoOriginalQuery(provaId1);
            var query2 = new ObterProvaAnoOriginalQuery(provaId2);

            var resultado1 = await handler.Handle(query1, CancellationToken.None);
            var resultado2 = await handler.Handle(query2, CancellationToken.None);

            Assert.Equal(anoOriginal1, resultado1);
            Assert.Equal(anoOriginal2, resultado2);
            Assert.NotEqual(resultado1, resultado2);

            mockRepositorio.Verify(r => r.ObterProvaAnoOriginal(provaId1), Times.Once);
            mockRepositorio.Verify(r => r.ObterProvaAnoOriginal(provaId2), Times.Once);
        }

        [Fact]
        public async Task Constructor_Deve_Aceitar_Repositorio()
        {
            var mockRepositorio = new Mock<IRepositorioProva>();

            var handler = new ObterProvaAnoOriginalQueryHandler(mockRepositorio.Object);

            Assert.NotNull(handler);
        }
    }
}
