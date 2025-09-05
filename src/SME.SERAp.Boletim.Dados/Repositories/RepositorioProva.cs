using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Repositories
{
    public class RepositorioProva : RepositorioBase<Prova>, IRepositorioProva
    {
        public RepositorioProva(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<ProvaDto>> ObterProvasFinalizadasPorData(DateTime data)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select 
                                p.id,
                                p.prova_legado_id as codigo,
                                p.descricao,
                                p.modalidade,
                                p.inicio,
                                p.fim,
                                p.data_correcao_inicio as dataCorrecaoInicio,
                                p.data_correcao_fim  as dataCorrecaoFim,
                                p.exibir_no_boletim,
                                p.formato_tai as formatoTai
                            from
                                prova p 
                            where 
                                p.data_correcao_fim  = @data and p.exibir_no_boletim";

                return await conn.QueryAsync<ProvaDto>(query, new { data });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<int?> ObterAnoProva(long provaId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select
	                            extract(year from p.inicio ) as ano
                            from
	                            prova p
                            where
	                            p.id = @provaId";

                return await conn.QueryFirstOrDefaultAsync<int?>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}