using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresPorProvaId
{
    public class ObterBoletinsEscolaresPorProvaIdQueryHandler : IRequestHandler<ObterBoletinsEscolaresPorProvaIdQuery, IEnumerable<BoletimEscolar>>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;
        public ObterBoletinsEscolaresPorProvaIdQueryHandler(IRepositorioBoletimEscolar repositorioBoletimEscolar)
        {
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
        }

        public Task<IEnumerable<BoletimEscolar>> Handle(ObterBoletinsEscolaresPorProvaIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimEscolar.ObterBoletinsEscolaresPorProvaId(request.ProvaId);
        }
    }
}
