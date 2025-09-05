using MediatR;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterAnoProvaQuery : IRequest<int?>
    {
        public long ProvaId { get; set; }
        public ObterAnoProvaQuery(long provaId)
        {
            ProvaId = provaId;
        }
    }
}
