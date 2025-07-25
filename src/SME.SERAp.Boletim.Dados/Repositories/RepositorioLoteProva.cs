using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Dominio.Enums;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Repositories
{
    public class RepositorioLoteProva : RepositorioBase<LoteProva>, IRepositorioLoteProva
    {
        public RepositorioLoteProva(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<int> DesativarTodosLotesProvaAtivos()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"UPDATE 
                                lote_prova 
                            SET 
                                exibir_no_boletim = false, 
                                data_alteracao = NOW()
                            WHERE 
                                exibir_no_boletim = true";

                return await conn.ExecuteAsync(query);
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

        public async Task<IEnumerable<LoteProva>> ObterLotesProvaPorData(DateTime inicio, DateTime fim, 
            bool formatoTai)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"SELECT
                                 lp.id,
                                 lp.nome,
                                 lp.tipo_tai as tipoTai,
                                 lp.exibir_no_boletim as exibirNoBoletim,
                                 lp.data_correcao_fim as dataCorrecaoFim,
                                 lp.data_inicio_lote as dataInicioLote,
                                 lp.data_criacao as dataCriacao,
                                 lp.data_alteracao as dataAlteracao
                                FROM
                                  lote_prova lp 
                                WHERE
	                                lp.data_inicio_lote between @inicio AND @fim
	                                AND lp.tipo_tai = @formatoTai";

                return await conn.QueryAsync<LoteProva>(query, new {inicio, fim, formatoTai });
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

        public async Task<int> AlterarStatusConsolidacao(long idLotProva, LoteStatusConsolidacao loteStatusConsolidacao)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"UPDATE 
                                lote_prova 
                            SET 
                                status_consolidacao = @loteStatusConsolidacao, 
                                data_alteracao = NOW()
                            WHERE 
                                id = @idLotProva";

                if(loteStatusConsolidacao == LoteStatusConsolidacao.Pendente)
                    query += " and status_consolidacao not in (1,2)";

                return await conn.ExecuteAsync(query, new { idLotProva, loteStatusConsolidacao });
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

        public async Task<IEnumerable<ProvaDto>> ObterProvasTaiAnoPorLoteId(long loteId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select
	                            p.id,
	                            blp.lote_id as loteId,
	                            p.modalidade,
	                            p.inicio,
	                            p.fim,
	                            p.data_correcao_inicio as dataCorrecaoInicio,
	                            p.data_correcao_fim as dataCorrecaoFim,
	                            p.exibir_no_boletim as exibirNoBoletim,
	                            p.formato_tai as formatoTai,
	                            p.descricao,
	                            pao.ano as anoEscolar,
                                p.disciplina_id as disciplinaId
                            from
	                            prova p
                            inner join boletim_lote_prova blp on
	                            blp.prova_id = p.id
                            inner join prova_ano_original pao on
	                            pao.prova_id = p.id
                            where
	                            blp.lote_id = @loteId 
	                            and p.exibir_no_boletim 
	                            and p.formato_tai";

                return await conn.QueryAsync<ProvaDto>(query, new { loteId });
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
    }
}
