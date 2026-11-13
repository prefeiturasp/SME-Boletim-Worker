using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioBoletimEscolar : IRepositorioBase<BoletimEscolar>
    {
        Task<IEnumerable<BoletimEscolar>> ObterBoletinsEscolaresPorProvaId(long provaId);
        Task<int> ExcluirBoletinsEscolaresPorProvaIdAsync(long provaId);
        Task<bool> ExisteBoletimEscolarAsync(long ueId, long provaId, long? disciplinaId, string? componenteCurricular);
    }
}