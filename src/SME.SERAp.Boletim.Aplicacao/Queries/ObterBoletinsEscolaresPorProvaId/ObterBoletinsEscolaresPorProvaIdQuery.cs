using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresPorProvaId
{
    public class ObterBoletinsEscolaresPorProvaIdQuery : IRequest<IEnumerable<BoletimEscolar>>
    {
        public ObterBoletinsEscolaresPorProvaIdQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
