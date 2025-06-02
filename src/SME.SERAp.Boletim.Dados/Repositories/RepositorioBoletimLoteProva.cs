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

        public async Task<IEnumerable<BoletimLoteProva>> ObterBoletimLoteProvaPendentes()
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
                            inner join lote_prova lp on 
                                lp.id = blp.lote_id
                            where lp.status_consolidacao = 1";

                return await conn.QueryAsync<BoletimLoteProva>(query);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
