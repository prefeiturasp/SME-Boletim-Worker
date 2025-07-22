using MediatR;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class ExcluirBoletimLoteUeCommand : IRequest<int>
    {
        public long LoteId { get; set; }

        public long UeId { get; set; }

        public int AnoEscolar { get; set; }

        public ExcluirBoletimLoteUeCommand(long loteId, long ueId, int anoEscolar)
        {
            LoteId = loteId;
            UeId = ueId;
            AnoEscolar = anoEscolar;
        }
    }
}
