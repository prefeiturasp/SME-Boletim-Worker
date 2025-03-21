using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioBoletimLoteProva : IRepositorioBase<BoletimLoteProva>
    {
        Task<IEnumerable<BoletimLoteProva>> ObterBoletimLoteProvaPorLoteId(long loteId);
    }
}
