using SME.SERAp.Boletim.Infra.Dtos.SerapEstudantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dados.Interfaces.SerapEstudantes
{
    public interface IRepositorioSerapProva
    {
        Task<IEnumerable<ProvaDto>> ObterTodosAsync();
        Task<IEnumerable<ProvaTurmaDto>> ObterTurmasAsync(long provaId);
        Task<SituacaoAlunoProvaDto> ObterSituacaoAlunoAsync(long provaId, long ra);
        Task<SituacaoTurmaProvaDto> ObterSituacaoTurmaAsync(long provaId, long turmaId);
        Task<ProvaTurmaDto> ObterTurmaAsync(long provaId, long turmaId);
        Task<IEnumerable<ProvaDto>> ObterProvasEmAndamentoAsync();
        Task<IEnumerable<long>> ObterDeficienciasAsync(long provaId);
        Task<IEnumerable<ProvaDto>> ObterProvasParaDeficientesAsync();
        Task<ProvaDto> ObterProvaParaDeficientePorProvaIdAsync(long provaId);
    }
}