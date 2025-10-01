using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioProva : IRepositorioBase<Prova>
    {
        Task<IEnumerable<ProvaDto>> ObterProvasFinalizadasPorData(DateTime data);

        Task<int?> ObterAnoProva(long provaId);
    }
}