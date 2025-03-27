using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioNivelProficiencia : IRepositorioBase<NivelProficiencia>
    {
        Task<IEnumerable<NivelProficiencia>> ObterNiveisProficienciaPorDisciplinaIdAnoEscolar(long disciplinaId, long anoEscolar);
    }
}
