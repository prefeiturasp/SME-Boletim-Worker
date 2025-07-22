using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioBoletimLoteUe : IRepositorioBase<BoletimLoteUe>
    {
        Task<int> ExcluirBoletimLoteUe(long loteId, long ueId, int anoEscolar);

        Task<IEnumerable<BoletimLoteUe>> ObterUesTotalAlunosPorLoteId(long loteId);

        Task<IEnumerable<BoletimLoteUeRealizaramProvaDto>> ObterUesAlunosRealizaramProvaPorLoteId(long loteId);
    }
}
