using Elasticsearch.Net;
using Moq;
using Nest;
using SME.SERAp.Boletim.Dados.Repositories.Elastic;
using SME.SERAp.Boletim.Dominio.Entities.Elastic;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Net;
using System.Reflection;

namespace SME.SERAp.Boletim.Dados.Testes.Repositories.Elastic
{
    public class EntidadeTeste : EntidadeBaseElastic
    {
        public string Nome { get; set; }
    }

    // Repositório fake que sobrescreve CriarIndexAsync
    public class RepositorioElasticTesteFake : RepositorioElasticBase<EntidadeTeste>
    {
        public RepositorioElasticTesteFake(ElasticOptions opcoes, IElasticClient cliente)
            : base(opcoes, cliente) { }

        public override Task<bool> CriarIndexAsync()
        {
            // Evita chamar Elastic real
            return Task.FromResult(true);
        }

        //public override Task<EntidadeTeste> ObterPorIdAsync(string id)
        //{
        //    return Task.FromResult(new EntidadeTeste { Id = id, Nome = "Teste" });
        //}
    }

    [Collection("ColecaoMapeamentos")]
    public class RepositorioElasticBaseTeste
    {
        private readonly Mock<IElasticClient> clienteElastic;
        private readonly RepositorioElasticTesteFake repositorio;
        private readonly ElasticOptions opcoes;

        public RepositorioElasticBaseTeste()
        {
            clienteElastic = new Mock<IElasticClient>();
            opcoes = new ElasticOptions { PrefixIndex = "teste" };

            repositorio = new RepositorioElasticTesteFake(opcoes, clienteElastic.Object);
        }

        [Fact]
        public async Task InserirAsync_Deve_Chamar_Index_Async()
        {
            var entidade = new EntidadeTeste { Id = "1", Nome = "Teste" };

            var respostaIndex = new Mock<IndexResponse>();
            respostaIndex.SetupGet(x => x.IsValid).Returns(true);

            clienteElastic
                .Setup(c => c.IndexAsync(entidade,
                    It.IsAny<Func<IndexDescriptor<EntidadeTeste>, IIndexRequest<EntidadeTeste>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(respostaIndex.Object);

            var resultado = await repositorio.InserirAsync(entidade);

            Assert.True(resultado);
            clienteElastic.VerifyAll();
        }

        [Fact]
        public async Task InserirAsync_Deve_Lancar_Excecao_Quando_Resposta_Invalida()
        {
            var entidade = new EntidadeTeste { Id = "1", Nome = "Teste" };

            var respostaIndex = new Mock<IndexResponse>();
            respostaIndex.SetupGet(x => x.IsValid).Returns(false);

            clienteElastic
                .Setup(c => c.IndexAsync(entidade,
                    It.IsAny<Func<IndexDescriptor<EntidadeTeste>, IIndexRequest<EntidadeTeste>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(respostaIndex.Object);

            await Assert.ThrowsAsync<Exception>(() => repositorio.InserirAsync(entidade));
        }

        [Fact]
        public async Task AlterarAsync_Deve_Chamar_Update_Async()
        {
            var entidade = new EntidadeTeste { Id = "1", Nome = "Teste" };

            var respostaUpdate = new Mock<UpdateResponse<EntidadeTeste>>();
            respostaUpdate.SetupGet(r => r.IsValid).Returns(true);

            clienteElastic.Setup(c => c.UpdateAsync<EntidadeTeste>(
                    It.IsAny<DocumentPath<EntidadeTeste>>(),
                    It.IsAny<Func<UpdateDescriptor<EntidadeTeste, EntidadeTeste>, IUpdateRequest<EntidadeTeste, EntidadeTeste>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(respostaUpdate.Object);

            var resultado = await repositorio.AlterarAsync(entidade);

            Assert.True(resultado);

            clienteElastic.Verify(c => c.UpdateAsync<EntidadeTeste>(
                It.IsAny<DocumentPath<EntidadeTeste>>(),
                It.IsAny<Func<UpdateDescriptor<EntidadeTeste, EntidadeTeste>, IUpdateRequest<EntidadeTeste, EntidadeTeste>>>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AlterarAsync_Deve_Lancar_Excecao_Quando_Resposta_Invalida()
        {
            var entidade = new EntidadeTeste { Id = "1", Nome = "Teste" };

            var respostaUpdate = new Mock<UpdateResponse<EntidadeTeste>>();
            respostaUpdate.SetupGet(r => r.IsValid).Returns(false);

            clienteElastic.Setup(c => c.UpdateAsync<EntidadeTeste>(
                    It.IsAny<DocumentPath<EntidadeTeste>>(),
                    It.IsAny<Func<UpdateDescriptor<EntidadeTeste, EntidadeTeste>, IUpdateRequest<EntidadeTeste, EntidadeTeste>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(respostaUpdate.Object);

            await Assert.ThrowsAsync<Exception>(() => repositorio.AlterarAsync(entidade));
        }

        [Fact]
        public async Task DeletarAsync_Deve_Chamar_Delete_Async()
        {
            var respostaDelete = new Mock<DeleteResponse>();
            respostaDelete.SetupGet(x => x.IsValid).Returns(true);

            clienteElastic
                .Setup(c => c.DeleteAsync<EntidadeTeste>(
                    It.IsAny<DocumentPath<EntidadeTeste>>(),
                    It.IsAny<Func<DeleteDescriptor<EntidadeTeste>, IDeleteRequest>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(respostaDelete.Object);

            var resultado = await repositorio.DeletarAsync("1");

            Assert.True(resultado);
            clienteElastic.VerifyAll();
        }

        [Fact]
        public async Task ObterPorIdAsync_Deve_Retornar_Entidade()
        {
            var entidade = new EntidadeTeste { Id = "1", Nome = "Teste" };
            var respostaGet = new Mock<GetResponse<EntidadeTeste>>();
            respostaGet.SetupGet(r => r.IsValid).Returns(true);

            var retorno = respostaGet.Object;

            DefinirPropriedadeSomenteGet(retorno, "Source", entidade);

            clienteElastic
                .Setup(c => c.GetAsync(It.IsAny<DocumentPath<EntidadeTeste>>(), It.IsAny<Func<GetDescriptor<EntidadeTeste>,IGetRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retorno);

            var resultado = await repositorio.ObterPorIdAsync(entidade.Id);

            Assert.NotNull(resultado);
            Assert.Equal("1", resultado.Id);
            Assert.Equal("Teste", resultado.Nome);
        }

        [Fact]
        public async Task ObterPorIdAsync_Deve_Retornar_Nulo_Quando_Resposta_Invalida()
        {
            var entidade = new EntidadeTeste { Id = "1", Nome = "Teste" };
            var respostaGet = new Mock<GetResponse<EntidadeTeste>>();
            respostaGet.SetupGet(r => r.IsValid).Returns(false);

            clienteElastic
                .Setup(c => c.GetAsync(It.IsAny<DocumentPath<EntidadeTeste>>(), It.IsAny<Func<GetDescriptor<EntidadeTeste>, IGetRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(respostaGet.Object);

            var resultado = await repositorio.ObterPorIdAsync(entidade.Id);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task ObterTodosAsync_Deve_Retornar_Lista_De_Entidades()
        {
            var entidade = new EntidadeTeste { Id = "1", Nome = "Teste" };

            var hitMock = new Mock<IHit<EntidadeTeste>>();
            hitMock.SetupGet(h => h.Source).Returns(entidade);

            var respostaPesquisa = new Mock<ISearchResponse<EntidadeTeste>>();
            respostaPesquisa.SetupGet(r => r.IsValid).Returns(true);
            respostaPesquisa.SetupGet(r => r.Hits).Returns(new[] { hitMock.Object });
            respostaPesquisa.SetupGet(r => r.ScrollId).Returns("scrollId");

            var respostaScroll = new Mock<ISearchResponse<EntidadeTeste>>();
            respostaScroll.SetupGet(r => r.IsValid).Returns(true);
            respostaScroll.SetupGet(r => r.Hits).Returns(Array.Empty<IHit<EntidadeTeste>>());

            clienteElastic
                .Setup(c => c.SearchAsync<EntidadeTeste>(It.IsAny<ISearchRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(respostaPesquisa.Object);

            clienteElastic
                .Setup(c => c.ScrollAsync<EntidadeTeste>(It.IsAny<Time>(), It.IsAny<string>(),
                    It.IsAny<Func<ScrollDescriptor<EntidadeTeste>, IScrollRequest>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(respostaScroll.Object);

            var resultado = (await repositorio.ObterTodosAsync()).ToList();

            Assert.Single(resultado);
            Assert.Equal("Teste", resultado[0].Nome);
            clienteElastic.VerifyAll();
        }

        [Fact]
        public async Task ObterTodosAsync_Deve_Lancar_Excecao_Quando_Resposta_Invalida()
        {
            var respostaPesquisa = new Mock<ISearchResponse<EntidadeTeste>>();
            respostaPesquisa.SetupGet(r => r.IsValid).Returns(false);

            clienteElastic
                .Setup(c => c.SearchAsync<EntidadeTeste>(It.IsAny<ISearchRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(respostaPesquisa.Object);

            await Assert.ThrowsAsync<Exception>(() => repositorio.ObterTodosAsync());
        }

        private static void DefinirPropriedadeSomenteGet<T>(T obj, string nomePropriedade, object valor)
        {
            var tipo = typeof(T);
            var campo = tipo.GetField($"<{nomePropriedade}>k__BackingField",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (campo == null)
                throw new InvalidOperationException($"Backing field de {nomePropriedade} não encontrado.");

            campo.SetValue(obj, valor);
        }
    }
}
