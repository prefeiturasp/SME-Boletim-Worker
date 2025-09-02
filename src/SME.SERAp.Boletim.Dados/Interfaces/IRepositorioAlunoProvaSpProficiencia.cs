using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioAlunoProvaSpProficiencia : IRepositorioBase<AlunoProvaSpProficiencia>
    {
        Task<ResultadoAlunoProvaSpDto> ObterResultadoAlunoProvaSp(int edicao, int areaDoConhecimento, string alunoMatricula);

        Task<AlunoProvaSpProficiencia> ObterAlunoProvaSpProficiencia(int anoLetivo, long disciplinaId, long alunoRa);

        Task<int> ExcluirAlunoProvaSpProficiencia(int anoLetivo, long disciplinaId, long alunoRa);
    }
}
