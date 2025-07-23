using SME.SERAp.Boletim.Infra.Dtos.Elastic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dados.Interfaces.Elastic
{
    public interface IRepositorioElasticProvaTurmaResultado
    {
        Task<ResumoGeralProvaDto> ObterResumoGeralPorUeAsync(long ueId, long provaId, int anoEscolar);
    }
}