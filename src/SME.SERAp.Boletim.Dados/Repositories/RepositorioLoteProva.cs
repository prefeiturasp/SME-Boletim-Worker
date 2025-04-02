using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
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
    }
}
