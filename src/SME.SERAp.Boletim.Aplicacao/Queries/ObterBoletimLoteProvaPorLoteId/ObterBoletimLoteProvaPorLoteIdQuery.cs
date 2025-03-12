using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimLoteProvaPorLoteId
{
    public class ObterBoletimLoteProvaPorLoteIdQuery : IRequest<IEnumerable<BoletimLoteProva>>
    {
        public ObterBoletimLoteProvaPorLoteIdQuery(long loteId)
        {
            LoteId = loteId;
        }

        public long LoteId { get; set; }
    }
}
