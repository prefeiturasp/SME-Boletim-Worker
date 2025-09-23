using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
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
	                            bpa.ano_escolar = @anoEscolar;";

                return await conn.QueryAsync<BoletimProvaAluno>(query, new { provaId, alunoRa, anoEscolar });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<BoletimEscolarDetalhesDto>> ObterBoletinsEscolaresDetalhesPorProvaId(long provaId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"with ue_proficiencia as (
                                                        select 
                                                            ue.id as ueId,
                                                            bpa.prova_id as provaId,
                                                            bpa.disciplina as disciplina,
                                                            bpa.disciplina_id as disciplinaId,
                                                            bpa.ano_escolar as anoEscolar,
                                                            count(case when bpa.nivel_codigo = 1 then 1 end) as abaixoBasico,
                                                            ROUND((COUNT(CASE WHEN bpa.nivel_codigo = 1 THEN 1 END) * 100.0) / COUNT(*), 2) AS abaixoBasicoPorcentagem,
                                                            count(case when bpa.nivel_codigo = 2 then 1 end) as basico,
                                                            ROUND((COUNT(CASE WHEN bpa.nivel_codigo = 2 THEN 1 END) * 100.0) / COUNT(*), 2) AS basicoPorcentagem,
                                                            count(case when bpa.nivel_codigo = 3 then 1 end) as adequado,
                                                            ROUND((COUNT(CASE WHEN bpa.nivel_codigo = 3 THEN 1 END) * 100.0) / COUNT(*), 2) AS adequadoPorcentagem,
                                                            count(case when bpa.nivel_codigo = 4 then 1 end) as avancado,
                                                            ROUND((COUNT(CASE WHEN bpa.nivel_codigo = 4 THEN 1 END) * 100.0) / COUNT(*), 2) AS avancadoPorcentagem,
                                                            count(*) as total,
                                                            ROUND(AVG(bpa.proficiencia), 2) AS mediaProficiencia
                                                        from
                                                            boletim_prova_aluno bpa
                                                        inner join ue on
                                                            ue.ue_id = bpa.ue_codigo      
                                                        where
                                                            bpa.prova_id = @provaId
                                                        group by
                                                            ue.id,
                                                            bpa.prova_id,
                                                            bpa.disciplina,
                                                            bpa.disciplina_id,
                                                            bpa.ano_escolar
                                                            )
                                                            select
                                                                up.*,
                                                                coalesce(nivel.codigo, 4) as nivelUeCodigo,
                                                                nivel.descricao as nivelUeDescricao
                                                            from ue_proficiencia up
                                                            left join lateral (
                                                                select np.codigo, np.descricao
                                                                from nivel_proficiencia np
                                                                where np.disciplina_id = up.disciplinaid
                                                                  and np.ano = up.anoescolar
                                                                  and (up.mediaproficiencia < np.valor_referencia or np.valor_referencia is null)
                                                                order by np.codigo asc
                                                                limit 1
                                                            ) nivel on true;";

                return await conn.QueryAsync<BoletimEscolarDetalhesDto>(query, new { provaId });
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
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
