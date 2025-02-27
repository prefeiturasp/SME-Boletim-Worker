using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Repositories
{
    public class RepositorioBoletimProvaAluno : RepositorioBase<BoletimProvaAluno>, IRepositorioBoletimProvaAluno
    {
        public RepositorioBoletimProvaAluno(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<BoletimProvaAluno>> ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolar(long provaId, long alunoRa, int anoEscolar)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select 
	                            * 
                            from 
	                            boletim_prova_aluno bpa
                            where 
	                            bpa.prova_id = @provaId and
	                            bpa.aluno_ra = @alunoRa and 
	                            bpa.ano_escolar = @anoEscolar";

                return await conn.QueryAsync<BoletimProvaAluno>(query, new { provaId, alunoRa, anoEscolar });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }


        public async Task<int> ExcluirPorIdAsync(long id)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"delete from boletim_prova_aluno where id = @id;";

                return await conn.ExecuteAsync(query, new { id });
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
