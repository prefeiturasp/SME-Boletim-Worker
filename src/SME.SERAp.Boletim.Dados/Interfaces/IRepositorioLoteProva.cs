using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Dominio.Enums;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioLoteProva : IRepositorioBase<LoteProva>
    {
        Task<int> DesativarTodosLotesProvaAtivos();
        Task<int> AlterarStatusConsolidacao(long idLotProva, LoteStatusConsolidacao loteStatusConsolidacao);
        Task<IEnumerable<LoteProva>> ObterLotesProvaPorData(DateTime inicio, DateTime fim, bool formataTai);
    }
}
