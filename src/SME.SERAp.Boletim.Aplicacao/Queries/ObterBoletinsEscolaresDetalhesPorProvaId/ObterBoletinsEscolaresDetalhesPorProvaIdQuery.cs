using MediatR;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresDetalhesPorProvaId
{
    public class ObterBoletinsEscolaresDetalhesPorProvaIdQuery : IRequest<IEnumerable<BoletimEscolarDetalhesDto>>
    {
        public ObterBoletinsEscolaresDetalhesPorProvaIdQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
