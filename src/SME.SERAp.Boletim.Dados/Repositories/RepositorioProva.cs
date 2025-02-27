using Dapper;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Dados.Repositories
{
    public class RepositorioProva : RepositorioBase<Prova>, IRepositorioProva
    {
        public RepositorioProva(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<ProvaDto>> ObterProvasFinalizadasPorData(DateTime data)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select 
	                            p.id,
	                            p.prova_legado_id as codigo,
	                            p.descricao,
	                            p.modalidade,
	                            p.inicio,
	                            p.fim,
	                            p.exibir_no_boletim
                            from
	                            prova p 
                            where 
	                            p.fim = @data and p.exibir_no_boletim";

                return await conn.QueryAsync<ProvaDto>(query, new { data });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}