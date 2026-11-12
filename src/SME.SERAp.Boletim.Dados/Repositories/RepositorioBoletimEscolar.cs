using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Repositories
{
    public class RepositorioBoletimEscolar : RepositorioBase<BoletimEscolar>, IRepositorioBoletimEscolar
    {
        public RepositorioBoletimEscolar(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<BoletimEscolar>> ObterBoletinsEscolaresPorProvaId(long provaId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select 
	                            * 
                            from 
	                            boletim_escolar be 
                            where 
	                            be.prova_id  = @provaId";

                return await conn.QueryAsync<BoletimEscolar>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<int> ExcluirBoletinsEscolaresPorProvaIdAsync(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"delete from boletim_escolar where prova_id = @provaId;";

                return await conn.ExecuteAsync(query, new { provaId });
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

        public async Task<bool> ExisteBoletimEscolarAsync(long ueId, long provaId, long? disciplinaId, string? componenteCurricular)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select exists (
                      select 1
                      from boletim_escolar
                      where ue_id = @ueId
                        and prova_id = @provaId
                        and disciplina_id is not distinct from @disciplinaId
                        and componente_curricular is not distinct from @componenteCurricular)";

                return await conn.ExecuteScalarAsync<bool>(query,
                    new { ueId, provaId, disciplinaId, componenteCurricular });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}