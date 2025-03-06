using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Repositories
{
    public class RepositorioAlunoProvaProficiencia : RepositorioBase<AlunoProvaProficiencia>, IRepositorioAlunoProvaProficiencia
    {
        public RepositorioAlunoProvaProficiencia(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<AlunoProvaProficienciaBoletimDto>> ObterAlunosProvaProficienciaBoletimPorProvaId(long provaId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"SELECT
                                u.dre_id as CodigoDre,
                                u.ue_id as CodigoUe,
                                u.nome as NomeUe,
                                p.id as ProvaId,
                                p.descricao as NomeProva,
                                NULLIF(REGEXP_REPLACE(t.ano, '[^0-9]', '', 'g'), '')::INTEGER as AnoEscolar,
                                t.nome as Turma,
                                a.ra as CodigoAluno,
                                a.nome as NomeAluno,
                                p.disciplina as NomeDisciplina,
                                p.disciplina_id as DisciplinaId,
                                app.tipo as ProvaStatus,
                                app.proficiencia as Proficiencia,
                                app.erro_medida as ErroMedida,
                                CASE
                                    WHEN NULLIF(REGEXP_REPLACE(t.ano, '[^0-9]', '', 'g'), '')::INTEGER = 5 AND p.disciplina::TEXT = 'Língua portuguesa' THEN
                                        CASE
                                            WHEN app.proficiencia < 150 THEN 1
                                            WHEN app.proficiencia < 200 THEN 2
                                            WHEN app.proficiencia < 250 THEN 3
                                            ELSE 4
                                        END
                                    WHEN NULLIF(REGEXP_REPLACE(t.ano, '[^0-9]', '', 'g'), '')::INTEGER = 5 AND p.disciplina::TEXT = 'Matemática' THEN
                                        CASE
                                            WHEN app.proficiencia < 175 THEN 1
                                            WHEN app.proficiencia < 225 THEN 2
                                            WHEN app.proficiencia < 275 THEN 3
                                            ELSE 4
                                        END
                                    WHEN NULLIF(REGEXP_REPLACE(t.ano, '[^0-9]', '', 'g'), '')::INTEGER = 9 AND p.disciplina::TEXT = 'Língua portuguesa' THEN
                                        CASE
                                            WHEN app.proficiencia < 200 THEN 1
                                            WHEN app.proficiencia < 275 THEN 2
                                            WHEN app.proficiencia < 325 THEN 3
                                            ELSE 4
                                        END
                                    WHEN NULLIF(REGEXP_REPLACE(t.ano, '[^0-9]', '', 'g'), '')::INTEGER = 9 AND p.disciplina::TEXT = 'Matemática' THEN
                                        CASE
                                            WHEN app.proficiencia < 225 THEN 1
                                            WHEN app.proficiencia < 300 THEN 2
                                            WHEN app.proficiencia < 350 then 3
                                            ELSE 4
                                        END
                                END AS NivelCodigo
                            FROM 
	                            aluno_prova_proficiencia app
                            INNER JOIN prova p ON 
	                            p.id = app.prova_id and 
	                            p.exibir_no_boletim 
                            INNER JOIN aluno a ON 
	                            a.id = app.aluno_id
                            INNER JOIN turma t ON 
	                            t.id = a.turma_id
                            INNER JOIN ue u ON 
	                            u.id = t.ue_id
                            WHERE 
	                            app.prova_id = @provaId AND 
	                            app.tipo IN ('2') AND 
	                            NULLIF(REGEXP_REPLACE(t.ano, '[^0-9]', '', 'g'), '')::INTEGER >= 5;";

                return await conn.QueryAsync<AlunoProvaProficienciaBoletimDto>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
