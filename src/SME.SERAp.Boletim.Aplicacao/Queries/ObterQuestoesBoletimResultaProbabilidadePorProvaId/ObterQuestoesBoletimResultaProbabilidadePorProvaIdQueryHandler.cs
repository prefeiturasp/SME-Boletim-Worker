using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterQuestoesBoletimResultaProbabilidadePorProvaId
{
    public class ObterQuestoesBoletimResultaProbabilidadePorProvaIdQueryHandler
        : IRequestHandler<ObterQuestoesBoletimResultaProbabilidadePorProvaIdQuery, IEnumerable<QuestaoProvaDto>>
    {
        private readonly IRepositorioBoletimResultadoProbabilidade repositorioBoletimResultadoProbabilidade;
        public ObterQuestoesBoletimResultaProbabilidadePorProvaIdQueryHandler(IRepositorioBoletimResultadoProbabilidade repositorioBoletimResultadoProbabilidade)
        {
            this.repositorioBoletimResultadoProbabilidade = repositorioBoletimResultadoProbabilidade;
        }

        public Task<IEnumerable<QuestaoProvaDto>> Handle(ObterQuestoesBoletimResultaProbabilidadePorProvaIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimResultadoProbabilidade.ObterQuestoesBoletimResultaProbabilidadePorProvaId(request.ProvaId);
        }
    }
}
