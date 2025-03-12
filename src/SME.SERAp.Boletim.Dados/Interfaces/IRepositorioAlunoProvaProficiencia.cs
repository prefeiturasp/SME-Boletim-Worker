using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioAlunoProvaProficiencia : IRepositorioBase<AlunoProvaProficiencia>
    {
        Task<IEnumerable<AlunoProvaProficienciaBoletimDto>> ObterAlunosProvaProficienciaBoletimPorProvaId(long provaId);
    }
}
