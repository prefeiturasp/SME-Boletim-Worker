using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Dominio.Enums;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioLoteProva : IRepositorioBase<LoteProva>
    {
        Task<int> DesativarTodosLotesProvaAtivos();
        Task<int> AlterarStatusConsolidacao(long idLotProva, LoteStatusConsolidacao loteStatusConsolidacao);
        Task<IEnumerable<LoteProva>> ObterLotesProvaPorData(DateTime inicio, DateTime fim, bool formataTai);
        Task<IEnumerable<ProvaDto>> ObterProvasTaiAnoPorLoteId(long loteId);
    }
}
