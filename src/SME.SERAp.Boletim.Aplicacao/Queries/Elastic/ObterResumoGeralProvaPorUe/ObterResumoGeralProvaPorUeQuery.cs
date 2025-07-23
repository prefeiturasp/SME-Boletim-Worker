using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.Elastic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.Elastic.ObterResumoGeralProvaPorUe
{
    public class ObterResumoGeralProvaPorUeQuery : IRequest<ResumoGeralProvaDto>
    {
        public ObterResumoGeralProvaPorUeQuery(long ueId, long provaId, int anoEscolar)
        {
            UeId = ueId;
            ProvaId = provaId;
            AnoEscolar = anoEscolar;
        }

        public long UeId { get; set; }
        public long ProvaId { get; set; }
        public int AnoEscolar { get; set; }
    }
}