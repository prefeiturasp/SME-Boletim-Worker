using Nest;
using SME.SERAp.Boletim.Dados.Interfaces.Elastic;
using SME.SERAp.Boletim.Dominio.Entities.Elastic;
using SME.SERAp.Boletim.Infra.Dtos.Elastic;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Repositories.Elastic
{
    public class RepositorioElasticProvaTurmaResultado : RepositorioElasticBase<ProvaTurmaResultado>, IRepositorioElasticProvaTurmaResultado
    {
        public RepositorioElasticProvaTurmaResultado(ElasticOptions elasticOptions, IElasticClient elasticClient) : base(elasticOptions, elasticClient)
        {
        }

        public async Task<ResumoGeralProvaDto> ObterResumoGeralPorUeAsync(long ueId, long provaId, int anoEscolar)
        {
            QueryContainer query = new QueryContainerDescriptor<ProvaTurmaResultado>()
                .Term(p => p.Field(f => f.ProvaId).Value(provaId))
                && new QueryContainerDescriptor<ProvaTurmaResultado>()
                .Term(p => p.Field(f => f.UeId).Value(ueId))
                 && new QueryContainerDescriptor<ProvaTurmaResultado>()
                .Term(p => p.Field(f => f.Ano).Value(anoEscolar));

            var search = new SearchDescriptor<ProvaTurmaResultado>(IndexName)
                .Query(_ => query)
                .Size(0)
                .Aggregations(a => a
                    .Sum("TotalAlunos", s => s.Field(f => f.TotalAlunos))
                    .Sum("ProvasIniciadas", s => s.Field(f => f.TotalIniciadas))
                    .Sum("ProvasNaoFinalizadas", s => s.Field(f => f.TotalNaoFinalizados))
                    .Sum("ProvasFinalizadas", s => s.Field(f => f.TotalFinalizados))
                    .Sum("TotalTempoMedio", s => s.Field(f => f.TempoMedio))
                    .Min("QtdeQuestoesProva", s => s.Field(f => f.QuantidadeQuestoes))
                    .Sum("TotalQuestoes", s => s.Field(f => f.TotalQuestoes))
                    .Sum("Respondidas", s => s.Field(f => f.QuestoesRespondidas))
                );

            var response = await elasticClient.SearchAsync<ProvaTurmaResultado>(search);
            if (!response.IsValid) return default;

            var resumoGeralProvaDto = new ResumoGeralProvaDto()
            {
                TotalAlunos = Convert.ToInt64(response.Aggregations.Sum("TotalAlunos")?.Value ?? 0),
                ProvasIniciadas = Convert.ToInt64(response.Aggregations.Sum("ProvasIniciadas")?.Value ?? 0),
                ProvasNaoFinalizadas = Convert.ToInt64(response.Aggregations.Sum("ProvasNaoFinalizadas")?.Value ?? 0),
                ProvasFinalizadas = Convert.ToInt64(response.Aggregations.Sum("ProvasFinalizadas")?.Value ?? 0),
                TotalTempoMedio = Convert.ToInt64(response.Aggregations.Sum("TotalTempoMedio")?.Value ?? 0),
                DetalheProva = new DetalheProvaDto()
                {
                    QtdeQuestoesProva = Convert.ToInt64(response.Aggregations.Min("QtdeQuestoesProva")?.Value ?? 0),
                    TotalQuestoes = Convert.ToDecimal(response.Aggregations.Sum("TotalQuestoes")?.Value ?? 0),
                    Respondidas = Convert.ToDecimal(response.Aggregations.Sum("Respondidas")?.Value ?? 0),
                },
                TotalTurmas = await ObterTotalTurmas(query)
            };

            return resumoGeralProvaDto;
        }

        public async Task<long> ObterTotalTurmas(QueryContainer query)
        {
            var searchTotalTurmas = new SearchDescriptor<ProvaTurmaResultado>(IndexName)
                .Query(q => !q.Term(p => p.TempoMedio, 0) && query)
                .Size(0)
                .Aggregations(a => a.ValueCount("TotalTurmas", c => c.Field(p => p.TurmaId)));

            var responseTotalTurmas = await elasticClient.SearchAsync<ProvaTurmaResultado>(searchTotalTurmas);
            if (!responseTotalTurmas.IsValid) return 0;

            var totalTurmas = responseTotalTurmas.Aggregations.ValueCount("TotalTurmas").Value.GetValueOrDefault();
            return (long)Math.Ceiling(totalTurmas);
        }
    }
}