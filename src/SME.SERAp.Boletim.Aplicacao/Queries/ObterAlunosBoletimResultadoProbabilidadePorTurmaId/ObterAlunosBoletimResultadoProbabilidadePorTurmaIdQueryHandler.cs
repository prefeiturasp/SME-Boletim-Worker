using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunosBoletimResultadoProbabilidadePorTurmaId
{
    public class ObterAlunosBoletimResultadoProbabilidadePorTurmaIdQueryHandler
        : IRequestHandler<ObterAlunosBoletimResultadoProbabilidadePorTurmaIdQuery, IEnumerable<AlunoBoletimResultadoProbabilidadeDto>>
    {
        private readonly IRepositorioBoletimResultadoProbabilidade repositorioBoletimResultadoProbabilidade;
        public ObterAlunosBoletimResultadoProbabilidadePorTurmaIdQueryHandler(IRepositorioBoletimResultadoProbabilidade repositorioBoletimResultadoProbabilidade)
        {
            this.repositorioBoletimResultadoProbabilidade = repositorioBoletimResultadoProbabilidade;
        }

        public Task<IEnumerable<AlunoBoletimResultadoProbabilidadeDto>> Handle(ObterAlunosBoletimResultadoProbabilidadePorTurmaIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimResultadoProbabilidade.ObterAlunosBoletimResultadoProbabilidadePorTurmaId(request.TurmaId, request.ProvaId);
        }
    }
}
