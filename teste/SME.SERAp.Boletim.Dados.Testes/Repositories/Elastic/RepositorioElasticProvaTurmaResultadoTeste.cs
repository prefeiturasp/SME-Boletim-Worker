using Moq;
using Nest;
using SME.SERAp.Boletim.Dados.Repositories.Elastic;
using SME.SERAp.Boletim.Dominio.Entities.Elastic;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Testes.Repositories.Elastic
{
    internal class RepositorioElasticProvaTurmaResultadoFake
        : RepositorioElasticProvaTurmaResultado
    {
        public RepositorioElasticProvaTurmaResultadoFake(ElasticOptions opt, IElasticClient client)
            : base(opt, client) { }

        public override Task<bool> CriarIndexAsync()
            => Task.FromResult(true);
    }

    public class RepositorioElasticProvaTurmaResultadoTeste
    {
        private readonly Mock<IElasticClient> elasticClient;
        private readonly RepositorioElasticProvaTurmaResultadoFake repositorio;
        public RepositorioElasticProvaTurmaResultadoTeste()
        {
            elasticClient = new Mock<IElasticClient>();

            var elasticOptions = new ElasticOptions { DefaultIndex = "prova-turma-resultado", PrefixIndex = "teste" };

            repositorio = new RepositorioElasticProvaTurmaResultadoFake(
                elasticOptions,
                elasticClient.Object
            );
        }

        [Fact]
        public async Task Deve_Obter_Resumo_Geral_Por_Ue_Async()
        {
            var ueId = 1L;
            var provaId = 10L;
            var anoEscolar = 5;

            var searchResponse = new Mock<ISearchResponse<ProvaTurmaResultado>>();
            searchResponse.SetupGet(r => r.IsValid).Returns(true);

            var aggregations = new AggregateDictionary(new Dictionary<string, IAggregate>
            {
                { "TotalAlunos", new ValueAggregate  { Value = 100 } },
                { "ProvasIniciadas", new ValueAggregate  { Value = 80 } },
                { "ProvasNaoFinalizadas", new ValueAggregate  { Value = 15 } },
                { "ProvasFinalizadas", new ValueAggregate  { Value = 65 } },
                { "TotalTempoMedio", new ValueAggregate  { Value = 500 } },
                { "QtdeQuestoesProva", new ValueAggregate  { Value = 20 } },
                { "TotalQuestoes", new ValueAggregate  { Value = 200 } },
                { "Respondidas", new ValueAggregate  { Value = 180 } }
            });

            searchResponse.SetupGet(r => r.Aggregations).Returns(aggregations);

            elasticClient
                .Setup(c => c.SearchAsync<ProvaTurmaResultado>(
                    It.IsAny<Func<SearchDescriptor<ProvaTurmaResultado>, ISearchRequest>>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(searchResponse.Object);

            var searchResponseTurmas = new Mock<ISearchResponse<ProvaTurmaResultado>>();
            searchResponseTurmas.SetupGet(r => r.IsValid).Returns(true);
            var aggsTurmas = new AggregateDictionary(new Dictionary<string, IAggregate>
            {
                { "TotalTurmas", new ValueAggregate { Value = 10 } }
            });

            searchResponseTurmas.SetupGet(r => r.Aggregations).Returns(aggsTurmas);

            elasticClient
                .SetupSequence(c => c.SearchAsync<ProvaTurmaResultado>(
                    It.IsAny<ISearchRequest>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(searchResponse.Object) // primeira chamada
                .ReturnsAsync(searchResponseTurmas.Object); // segunda chamada

            var resultado = await repositorio.ObterResumoGeralPorUeAsync(ueId, provaId, anoEscolar);

            Assert.NotNull(resultado);
            Assert.Equal(100, resultado.TotalAlunos);
            Assert.Equal(80, resultado.ProvasIniciadas);
            Assert.Equal(15, resultado.ProvasNaoFinalizadas);
            Assert.Equal(65, resultado.ProvasFinalizadas);
            Assert.Equal(500, resultado.TotalTempoMedio);
            Assert.Equal(20, resultado.DetalheProva.QtdeQuestoesProva);
            Assert.Equal(200, resultado.DetalheProva.TotalQuestoes);
            Assert.Equal(180, resultado.DetalheProva.Respondidas);
            Assert.Equal(10, resultado.TotalTurmas);
        }

        [Fact]
        public async Task Deve_Obter_Total_Turmas()
        {
            var searchTurmas = new Mock<ISearchResponse<ProvaTurmaResultado>>();
            searchTurmas.SetupGet(r => r.IsValid).Returns(true);

            var aggsTurmas = new AggregateDictionary(new Dictionary<string, IAggregate>
            {
                { "TotalTurmas", new ValueAggregate { Value = 10 } }
            });

            searchTurmas.SetupGet(r => r.Aggregations).Returns(aggsTurmas);

            elasticClient
                .Setup(c => c.SearchAsync<ProvaTurmaResultado>(
                    It.IsAny<ISearchRequest>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(searchTurmas.Object);

            var total = await repositorio.ObterTotalTurmas(new QueryContainer());

            Assert.Equal(10, total);
            elasticClient.VerifyAll();
        }

        [Fact]
        public async Task Deve_Obter_Total_Turmas_Arredondado_Para_Cima()
        {
            var searchTurmas = new Mock<ISearchResponse<ProvaTurmaResultado>>();
            searchTurmas.SetupGet(r => r.IsValid).Returns(true);

            var aggsTurmas = new AggregateDictionary(new Dictionary<string, IAggregate>
            {
                { "TotalTurmas", new ValueAggregate { Value = 7.2 } }
            });

            searchTurmas.SetupGet(r => r.Aggregations).Returns(aggsTurmas);

            elasticClient
                .Setup(c => c.SearchAsync<ProvaTurmaResultado>(
                    It.IsAny<ISearchRequest>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(searchTurmas.Object);

            var total = await repositorio.ObterTotalTurmas(new QueryContainer());

            Assert.Equal(8, total);
            elasticClient.VerifyAll();
        }
    }
}
