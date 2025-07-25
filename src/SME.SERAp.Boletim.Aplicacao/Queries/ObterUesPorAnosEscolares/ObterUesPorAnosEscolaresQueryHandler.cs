using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterUesPorAnosEscolaresQueryHandler : IRequestHandler<ObterUesPorAnosEscolaresQuery, IEnumerable<UeDto>>
    {
        private readonly IRepositorioBoletimLoteUe repositorioBoletimLoteUe;
        public ObterUesPorAnosEscolaresQueryHandler(IRepositorioBoletimLoteUe repositorioBoletimLoteUe)
        {
            this.repositorioBoletimLoteUe = repositorioBoletimLoteUe;
        }

        public Task<IEnumerable<UeDto>> Handle(ObterUesPorAnosEscolaresQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioBoletimLoteUe.ObterUesPorAnosEscolares(request.AnosEscolares, request.AnoLetivo);
        }
    }
}
