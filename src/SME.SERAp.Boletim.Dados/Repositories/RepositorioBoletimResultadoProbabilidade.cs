using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Repositories
{
    public class RepositorioBoletimResultadoProbabilidade
        : RepositorioBase<BoletimResultadoProbabilidade>, IRepositorioBoletimResultadoProbabilidade
    {
        public RepositorioBoletimResultadoProbabilidade(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<int> ExcluirBoletinsResultadosProbabilidadesPorProvaIdAsync(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"delete from boletim_resultado_probabilidade where prova_id = @provaId;";

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

        public async Task<IEnumerable<TurmaBoletimResultadoProbabilidadeDto>> ObterTurmasBoletimResultadoProbabilidadePorProvaId(long provaId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select 
	                            p.id as provaId, 
	                            u.id as ueId,
	                            t.id as turmaId,
	                            t.nome as turmaNome,
	                            t.ano_letivo as anoLetivo,
	                            p.disciplina_id as disciplinaId,
	                            t.ano as anoEscolar
                            from 
	                            boletim_prova_aluno bpa
                            inner join prova p on
	                            p.id = bpa.prova_id
                            inner join ue u on
	                            u.ue_id = bpa.ue_codigo
                            inner join turma t on
	                            t.ue_id = u.id and
	                            t.nome = bpa.turma and
	                            t.ano_letivo = EXTRACT(YEAR FROM p.inicio)
                            where 
	                            bpa.prova_id = @provaId
                            group by 
	                            p.id, 
	                            u.id,
	                            t.id,
	                            t.nome,
	                            t.ano_letivo,
	                            p.disciplina_id,
	                            t.ano
                            order by 
	                            p.id,
	                            t.nome, 
	                            u.id,
	                            t.id";

                return await conn.QueryAsync<TurmaBoletimResultadoProbabilidadeDto>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<AlunoBoletimResultadoProbabilidadeDto>> ObterAlunosBoletimResultadoProbabilidadePorTurmaId(long turmaId, long provaId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select 
	                            a.id,
	                            a.nome,
	                            a.ra as codigo,
	                            a.turma_id as turmaId,
	                            t.nome as turmaNome,
	                            u.ue_id as ueId,
	                            case when bpa.proficiencia is not null then
		                            bpa.proficiencia
	                            else
		                            0
	                            end as proficiencia
                            from 
	                            aluno a
                            inner join turma t on
	                            t.id = a.turma_id
                            inner join ue u on
	                            u.id = t.ue_id
                            left join boletim_prova_aluno bpa on
	                            bpa.aluno_ra = a.ra and
	                            bpa.turma = t.nome and
	                            bpa.ue_codigo = u.ue_id and
	                            bpa.dre_id = u.dre_id and
	                            bpa.prova_id = @provaId
                            where
	                            t.id = @turmaId";

                return await conn.QueryAsync<AlunoBoletimResultadoProbabilidadeDto>(query, new { turmaId, provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<QuestaoProvaDto>> ObterQuestoesBoletimResultaProbabilidadePorProvaId(long provaId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select
	                            h.descricao as descricaoHabilidade,
	                            h.codigo as codigoHabilidade,
	                            h.id as habilidadeId,
	                            q.questao_legado_id as questaoLegadoId,
	                            qt.discriminacao,
	                            qt.dificuldade,
	                            qt.acerto_casual as acertoCasual
                            from
	                            public.questao q
                            inner join questao_tri qt on
	                            qt.questao_id = q.id
                            inner join habilidade h on
	                            h.id = q.habilidade_legado_id
                            where
	                            q.prova_id = @provaId";

                return await conn.QueryAsync<QuestaoProvaDto>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
