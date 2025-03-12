using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresDetalhesPorProvaId
{
    public class ObterBoletinsEscolaresDetalhesPorProvaIdQueryHandler : IRequestHandler<ObterBoletinsEscolaresDetalhesPorProvaIdQuery, IEnumerable<BoletimEscolarDetalhesDto>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;
        public ObterBoletinsEscolaresDetalhesPorProvaIdQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
        }

        public Task<IEnumerable<BoletimEscolarDetalhesDto>> Handle(ObterBoletinsEscolaresDetalhesPorProvaIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimProvaAluno.ObterBoletinsEscolaresDetalhesPorProvaId(request.ProvaId);
        }
    }
}
