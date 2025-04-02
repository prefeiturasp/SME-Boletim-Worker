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
	                            blp.prova_id as provaId
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
    }
}
