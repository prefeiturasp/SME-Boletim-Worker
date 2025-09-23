using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Repositories
{
    public class RepositorioNivelProficiencia : RepositorioBase<NivelProficiencia>, IRepositorioNivelProficiencia
    {
        public RepositorioNivelProficiencia(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<NivelProficiencia>> ObterNiveisProficienciaPorDisciplinaIdAnoEscolar(long disciplinaId, long anoEscolar)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select
	                            np.id,
	                            np.codigo,
	                            np.descricao,
	                            np.valor_referencia as valorReferencia,
	                            np.disciplina_id as disciplinaId,
	                            np.ano
                            from
	                            nivel_proficiencia np
                            where
	                            np.disciplina_id = @disciplinaId
	                            and np.ano = @anoEscolar";

                return await conn.QueryAsync<NivelProficiencia>(query, new { disciplinaId, anoEscolar });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
