using MediatR;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterUesAlunosRealizaramProvaPorLoteIdQuery : IRequest<IEnumerable<BoletimLoteUeRealizaramProvaDto>>
    {
        public ObterUesAlunosRealizaramProvaPorLoteIdQuery(long loteId)
        {
            LoteId = loteId;
        }
        public long LoteId { get; }
    }
}
