using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterUesTotalAlunosPorLoteIdQuery : IRequest<IEnumerable<BoletimLoteUe>>
    {
        public long LoteId { get; set; }
        public ObterUesTotalAlunosPorLoteIdQuery(long loteId)
        {
            LoteId = loteId;
        }
    }
}
