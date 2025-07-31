using MediatR;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterProvasTaiPorLoteIdQuery : IRequest<IEnumerable<ProvaDto>>
    {
        public long LoteId { get; set; }
        public ObterProvasTaiPorLoteIdQuery(long loteId)
        {
            LoteId = loteId;
        }
    }
}
