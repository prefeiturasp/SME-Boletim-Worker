using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces.SerapEstudantes;
using SME.SERAp.Boletim.Infra.Dtos.SerapEstudantes;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dados.Repositories.SerapEstudantes
{
    public class RepositorioSerapProva : RepositorioSerap, IRepositorioSerapProva
    {
        public RepositorioSerapProva(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<SituacaoAlunoProvaDto> ObterSituacaoAlunoAsync(long provaId, long ra)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select min(pa.criado_em) as Inicio,
                                     max(case when pa.status in (2, 5) then pa.finalizado_em end) as Fim,
                                     count(distinct tb.questao_id) as QuestaoRespondida,
                                     sum(tb.tempo_resposta_aluno) Tempo,
                                     exists(select 1 from downloads_prova_aluno dpa where dpa.aluno_ra = pa.aluno_ra and dpa.prova_id = pa.prova_id limit 1) as FezDownload,
                                     cast((select par.usuario_id_coresso from prova_aluno_reabertura par where par.prova_id = pa.prova_id and par.aluno_ra = pa.aluno_ra order by par.criado_em desc limit 1) as varchar(40)) as UsuarioIdReabertura,
                                     (select par.criado_em from prova_aluno_reabertura par where par.prova_id = pa.prova_id and par.aluno_ra = pa.aluno_ra order by par.criado_em desc limit 1) as DataHoraReabertura
                              from prova_aluno pa
                              left join ( select q.prova_id, qar.aluno_ra, qar.tempo_resposta_aluno, qar.alternativa_id, qar.questao_id
	                                      from questao_aluno_resposta qar 
	                                      left join questao q on q.id = qar.questao_id) tb on tb.prova_id = pa.prova_id and tb.aluno_ra = pa.aluno_ra
                              where pa.aluno_ra = @ra 
                                and pa.prova_id = @provaId	
                              group by pa.aluno_ra, pa.prova_id";


                return await conn.QueryFirstOrDefaultAsync<SituacaoAlunoProvaDto>(query, new { provaId, ra });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<SituacaoTurmaProvaDto> ObterSituacaoTurmaAsync(long provaId, long turmaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select count(case when pa.criado_em::date = current_date and pa.status = 1 then 1 end) as totalIniciadoHoje,
	                                 count(case when pa.criado_em::date < current_date and pa.status = 1 then 1 end) as totalIniciadoNaoFinalizado,
	                                 count(case when pa.status in (2, 5) then 1 end) as totalFinalizado
                              from prova_aluno pa
                              left join aluno a on a.ra = pa.aluno_ra
                              where pa.prova_id = @provaId and a.turma_id = @turmaId";

                return await conn.QueryFirstOrDefaultAsync<SituacaoTurmaProvaDto>(query, new { provaId, turmaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ProvaDto>> ObterTodosAsync()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select p.id, 
	                                 p.prova_legado_id as codigo, 
	                                 p.descricao, 
                                     p.modalidade,
	                                 p.inicio::date, 
	                                 p.fim::date,
                                     p.total_itens as QuantidadeQuestoes
                              from prova p 
                              where (p.ocultar_prova = false or p.ocultar_prova is null)";

                return await conn.QueryAsync<ProvaDto>(query);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ProvaDto>> ObterProvasEmAndamentoAsync()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select p.id, 
	                                 p.prova_legado_id as codigo, 
	                                 p.descricao, 
                                     p.modalidade,
	                                 p.inicio::date, 
	                                 p.fim::date,
                                     p.total_itens as QuantidadeQuestoes
                              from prova p 
                              where inicio::date <= current_date and fim::date >= (current_date -1)
                                and (p.ocultar_prova = false or p.ocultar_prova is null)";

                return await conn.QueryAsync<ProvaDto>(query);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ProvaTurmaDto>> ObterTurmasAsync(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select distinct vpta.turma_ano_letivo as anoLetivo,
                                   vpta.prova_id as provaId,
	                               vpta.inicio::date,
	                               vpta.fim::date,
	                               u.dre_id as dreId,
	                               vpta.ue_id as ueId,
	                               vpta.turma_ano as ano,
	                               vpta.turma_modalidade as modalidade,
	                               vpta.turma_id as TurmaId,
                                   p.descricao,
	                               p.total_itens as QuantidadeQuestoes,
                                   p.aderir_todos as AderirTodos,
                                   tp.para_estudante_com_deficiencia as Deficiente
                             from v_prova_turma_aluno vpta 
                             left join prova p on p.id = vpta.prova_id 
                             left join ue u on u.id = vpta.ue_id
                             left join tipo_prova tp on tp.id = p.tipo_prova_id
                             where vpta.prova_id = @provaId ";

                return await conn.QueryAsync<ProvaTurmaDto>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<ProvaTurmaDto> ObterTurmaAsync(long provaId, long turmaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select distinct vpta.turma_ano_letivo as anoLetivo,
                                   vpta.prova_id as provaId,
	                               vpta.inicio,
	                               vpta.fim,
	                               u.dre_id as dreId,
	                               vpta.ue_id as ueId,
	                               vpta.turma_ano as ano,
	                               vpta.turma_modalidade as modalidade,
	                               vpta.turma_id as TurmaId,
                                   p.descricao,
	                               p.total_itens as QuantidadeQuestoes
                             from v_prova_turma_aluno vpta 
                             left join prova p on p.id = vpta.prova_id 
                             left join ue u on u.id = vpta.ue_id
                             where vpta.prova_id = @provaId 
                               and vpta.turma_id = @turmaId ";

                return await conn.QueryFirstOrDefaultAsync<ProvaTurmaDto>(query, new { provaId, turmaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<long>> ObterDeficienciasAsync(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select td.codigo_eol
                              from prova p
                              inner join tipo_prova tp on tp.id = p.tipo_prova_id
                              inner join tipo_prova_deficiencia tpd on tpd.tipo_prova_id = tp.id  
                              inner join tipo_deficiencia td on td.id = tpd.deficiencia_id 
                              where p.id = @provaId";

                return await conn.QueryAsync<long>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ProvaDto>> ObterProvasParaDeficientesAsync()
        {
            using var conn = ObterConexao();
            try
            {
                const string query = @"select p.id, 
        	                                 p.prova_legado_id as codigo, 
        	                                 p.descricao, 
                                             p.modalidade,
	                                         p.inicio::date, 
	                                         p.fim::date,
                                             p.total_itens as QuantidadeQuestoes
                                        from prova p 
                                        inner join tipo_prova tp on tp.id = p.tipo_prova_id
                                        where (p.ocultar_prova = false or p.ocultar_prova is null)
                                        and tp.para_estudante_com_deficiencia";

                return await conn.QueryAsync<ProvaDto>(query);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<ProvaDto> ObterProvaParaDeficientePorProvaIdAsync(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                const string query = @"select p.id, 
        	                                 p.prova_legado_id as codigo, 
        	                                 p.descricao, 
                                             p.modalidade,
	                                         p.inicio::date, 
	                                         p.fim::date,
                                             p.total_itens as QuantidadeQuestoes
                                        from prova p 
                                        inner join tipo_prova tp on tp.id = p.tipo_prova_id
                                        where p.id = @provaId 
                                        and (p.ocultar_prova = false or p.ocultar_prova is null)
                                        and tp.para_estudante_com_deficiencia";

                return await conn.QueryFirstOrDefaultAsync<ProvaDto>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}