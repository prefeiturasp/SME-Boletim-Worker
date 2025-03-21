using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioLoteProva : IRepositorioBase<LoteProva>
    {
        Task<int> DesativarTodosLotesProvaAtivos();
        Task<IEnumerable<LoteProva>> ObterLotesProvaPorData(DateTime inicio, DateTime fim, bool formataTai);
    }
}
