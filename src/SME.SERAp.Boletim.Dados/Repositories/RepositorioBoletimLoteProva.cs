using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Repositories
{
    public class RepositorioBoletimLoteProva : RepositorioBase<BoletimLoteProva>, IRepositorioBoletimLoteProva
    {
        public RepositorioBoletimLoteProva(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<BoletimLoteProva>> ObterBoletimLoteProvaPorLoteId(long loteId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select
	                            blp.id,
	                            blp.lote_id as loteId,
	                            blp.prova_id as provaId,
	                            blp.exibir_no_boletim as exibirNoBoletim
                            from 
	                            boletim_lote_prova blp 
                            where blp.lote_id = @loteId";

                return await conn.QueryAsync<BoletimLoteProva>(query, new { loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<long> ObterUltimoBoletimLoteId()
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select 
	                            case when max(blp.lote_id) is null then 
                                    0 
                                else
                                    max(blp.lote_id) 
                                end as ultimoProvaId  
                            from 
	                            boletim_lote_prova blp";

                return await conn.QueryFirstAsync<long>(query);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<int> DesativarTodosBoletimLotes()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"update boletim_lote_prova set exibir_no_boletim = false where exibir_no_boletim = true";

                return await conn.ExecuteAsync(query);
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
