using MediatR;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterTurmasBoletimResultadoProbabilidadePorProvaId
{
    public class ObterTurmasBoletimResultadoProbabilidadePorProvaIdQuery : IRequest<IEnumerable<TurmaBoletimResultadoProbabilidadeDto>>
    {
        public ObterTurmasBoletimResultadoProbabilidadePorProvaIdQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
