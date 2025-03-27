using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioBoletimResultadoProbabilidade : IRepositorioBase<BoletimResultadoProbabilidade>
    {
        Task<int> ExcluirBoletinsResultadosProbabilidadesPorProvaIdAsync(long provaId);

        Task<IEnumerable<TurmaBoletimResultadoProbabilidadeDto>> ObterTurmasBoletimResultadoProbabilidadePorProvaId(long provaId);

        Task<IEnumerable<AlunoBoletimResultadoProbabilidadeDto>> ObterAlunosBoletimResultadoProbabilidadePorTurmaId(long turmaId, long provaId);

        Task<IEnumerable<QuestaoProvaDto>> ObterQuestoesBoletimResultaProbabilidadePorProvaId(long provaId);
    }
}
