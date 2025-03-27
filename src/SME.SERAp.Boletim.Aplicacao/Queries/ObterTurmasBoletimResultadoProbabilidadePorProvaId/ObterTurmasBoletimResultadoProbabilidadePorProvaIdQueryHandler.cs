using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterTurmasBoletimResultadoProbabilidadePorProvaId
{
    public class ObterTurmasBoletimResultadoProbabilidadePorProvaIdQueryHandler
        : IRequestHandler<ObterTurmasBoletimResultadoProbabilidadePorProvaIdQuery, IEnumerable<TurmaBoletimResultadoProbabilidadeDto>>
    {
        private readonly IRepositorioBoletimResultadoProbabilidade repositorioBoletimResultadoProbabilidade;
        public ObterTurmasBoletimResultadoProbabilidadePorProvaIdQueryHandler(IRepositorioBoletimResultadoProbabilidade repositorioBoletimResultadoProbabilidade)
        {
            this.repositorioBoletimResultadoProbabilidade = repositorioBoletimResultadoProbabilidade;
        }

        public Task<IEnumerable<TurmaBoletimResultadoProbabilidadeDto>> Handle(ObterTurmasBoletimResultadoProbabilidadePorProvaIdQuery request, CancellationToken cancellationToken)
        {
           return repositorioBoletimResultadoProbabilidade.ObterTurmasBoletimResultadoProbabilidadePorProvaId(request.ProvaId);
        }
    }
}
