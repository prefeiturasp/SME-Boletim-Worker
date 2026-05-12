using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioBoletimProvaAluno : IRepositorioBase<BoletimProvaAluno>
    {
        Task<int> ExcluirPorIdAsync(long id);
        Task<IEnumerable<BoletimProvaAluno>> ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolar(long provaId, long alunoRa, int anoEscolar);
        Task<IEnumerable<BoletimEscolarDetalhesDto>> ObterBoletinsEscolaresDetalhesPorProvaId(long provaId);
        Task<BoletimProvaAlunoUltimaTurmaAlunoDto> ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(long alunRa, int anoLetivo, int anoEscolar, long disciplinaId, decimal proficiencia);
    }
}
