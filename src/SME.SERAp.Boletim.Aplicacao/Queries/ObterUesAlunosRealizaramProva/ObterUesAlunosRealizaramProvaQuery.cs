using MediatR;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterUesAlunosRealizaramProvaQuery : IRequest<BoletimLoteUeRealizaramProvaDto>
    {
        public long LoteId { get; set; }

        public long UeId { get; set; }

        public int AnoEscolar { get; set; }

        public ObterUesAlunosRealizaramProvaQuery(long loteId, long ueId, int anoEscolar)
        {
            LoteId = loteId;
            UeId = ueId;
            AnoEscolar = anoEscolar;
        }
    }
}
