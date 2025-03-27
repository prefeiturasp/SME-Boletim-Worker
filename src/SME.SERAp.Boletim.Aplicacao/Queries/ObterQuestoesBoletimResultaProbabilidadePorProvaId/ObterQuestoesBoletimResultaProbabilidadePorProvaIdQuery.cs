using MediatR;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterQuestoesBoletimResultaProbabilidadePorProvaId
{
    public class ObterQuestoesBoletimResultaProbabilidadePorProvaIdQuery : IRequest<IEnumerable<QuestaoProvaDto>>
    {
        public ObterQuestoesBoletimResultaProbabilidadePorProvaIdQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
