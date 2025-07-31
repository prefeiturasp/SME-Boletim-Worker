using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Repositories
{
    public class RepositorioBoletimLoteUe : RepositorioBase<BoletimLoteUe>, IRepositorioBoletimLoteUe
    {
        public RepositorioBoletimLoteUe(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<BoletimLoteUe>> ObterUesTotalAlunosPorLoteId(long loteId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select
	                            u.dre_id as dreId,
	                            u.id as ueId,
	                            blp.lote_id as loteId,
	                            vpta.turma_ano::int as anoEscolar,
	                            count(distinct a.ra) as totalAlunos
                            from
	                            v_prova_turma_aluno vpta
                            inner join aluno a on
	                            a.id = vpta.aluno_id
                            inner join boletim_lote_prova blp on
	                            blp.prova_id = vpta.prova_id
                            inner join ue u on
	                            u.id = vpta.ue_id
                            left join aluno_deficiencia ad on
	                            ad.aluno_ra = a.ra
                            left join tipo_deficiencia td on
	                            td.id = ad.deficiencia_id
                            where
	                            blp.lote_id = @loteId
	                            and (td.id is null
		                            or td.prova_normal)
	                            and vpta.aluno_situacao <> 99
                            group by
	                            u.dre_id,
	                            u.id,
	                            blp.lote_id,
	                            vpta.turma_ano";

                return await conn.QueryAsync<BoletimLoteUe>(query, new { loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<BoletimLoteUeRealizaramProvaDto>> ObterUesAlunosRealizaramProvaPorLoteId(long loteId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select
	                            u.dre_id as dreId,
	                            u.id as ueId,
	                            blp.lote_id as loteId,
	                            bpa.ano_escolar as anoEscolar,
                                count(distinct bpa.aluno_ra) as realizaramProva
                            from
                                ue u
                            inner join boletim_prova_aluno bpa on
                                bpa.ue_codigo = u.ue_id
                            inner join boletim_lote_prova blp on
                                blp.prova_id = bpa.prova_id
                            where
                                blp.lote_id = @loteId
                            group by
                                u.dre_id,
	                            u.id,
	                            blp.lote_id,
	                            bpa.ano_escolar";

                return await conn.QueryAsync<BoletimLoteUeRealizaramProvaDto>(query, new { loteId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<int> ExcluirBoletimLoteUe(long loteId, long ueId, int anoEscolar)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"delete from boletim_lote_ue blu
                                        where blu.lote_id = @loteId and blu.ue_id = @ueId and blu.ano_escolar = @anoEscolar";

                return await conn.ExecuteAsync(query, new { loteId, ueId, anoEscolar });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<UeDto>> ObterUesPorAnosEscolares(IEnumerable<string> anosEscolares, int anoLetivo)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select
	                            u.id,
	                            u.dre_id as dreId,
	                            u.nome
                            from
	                            ue u
                            inner join turma t on
	                            t.ue_id = u.id
                            where
	                            t.ano_letivo = @anoLetivo
	                            and t.ano = ANY(@anosEscolares)
                            group by
	                            u.id,
	                            u.dre_id,
	                            u.nome";

                return await conn.QueryAsync<UeDto>(query, new { anosEscolares, anoLetivo });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<BoletimLoteUeRealizaramProvaDto> ObterUesAlunosRealizaramProva(long loteId, long ueId, int anoEscolar)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select
                                u.dre_id as dreId,
                                u.id as ueId,
                                blp.lote_id as loteId,
                                bpa.ano_escolar as anoEscolar,
                                count(distinct bpa.aluno_ra) as realizaramProva
                            from
                                ue u
                            inner join boletim_prova_aluno bpa on
                                bpa.ue_codigo = u.ue_id
                            inner join boletim_lote_prova blp on
                                blp.prova_id = bpa.prova_id
                            where
                                blp.lote_id = @loteId
                                and bpa.ano_escolar = @anoEscolar
                                and u.id = @ueId
                            group by
                                u.dre_id,
                                u.id,
                                blp.lote_id,
                                bpa.ano_escolar";

                return await conn.QueryFirstOrDefaultAsync<BoletimLoteUeRealizaramProvaDto>(query, new { loteId, ueId, anoEscolar });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
