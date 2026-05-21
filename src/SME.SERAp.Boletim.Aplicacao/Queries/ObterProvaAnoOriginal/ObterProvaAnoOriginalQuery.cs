using MediatR;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaAnoOriginal
{
    public class ObterProvaAnoOriginalQuery : IRequest<string>
    {
        public long ProvaId { get; set; }

        public ObterProvaAnoOriginalQuery(long provaId)
        {
            ProvaId = provaId;
        }
    }
}
