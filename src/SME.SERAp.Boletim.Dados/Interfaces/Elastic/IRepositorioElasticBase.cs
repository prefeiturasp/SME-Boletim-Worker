using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Dominio.Entities.Elastic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dados.Interfaces.Elastic
{
    public interface IRepositorioElasticBase<TEntidade> where TEntidade : EntidadeBaseElastic
    {
        Task<bool> AlterarAsync(TEntidade entidade);
        Task<bool> CriarIndexAsync();
        Task<bool> DeletarAsync(string id);
        Task<bool> InserirAsync(TEntidade entidade);
        Task<TEntidade> ObterPorIdAsync(string id);
        Task<IEnumerable<TEntidade>> ObterTodosAsync();
    }
}