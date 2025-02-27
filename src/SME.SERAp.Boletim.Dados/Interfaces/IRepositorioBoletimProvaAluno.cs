using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioBoletimProvaAluno : IRepositorioBase<BoletimProvaAluno>
    {
        Task<int> ExcluirPorIdAsync(long id);
        Task<IEnumerable<BoletimProvaAluno>> ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolar(long provaId, long alunoRa, int anoEscolar);
    }
}
