using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterUesAlunosRealizaramProvaPorLoteIdQueryHandler : IRequestHandler<ObterUesAlunosRealizaramProvaPorLoteIdQuery, IEnumerable<BoletimLoteUeRealizaramProvaDto>>
    {
        private readonly IRepositorioBoletimLoteUe repositorioBoletimLoteUe;
        public ObterUesAlunosRealizaramProvaPorLoteIdQueryHandler(IRepositorioBoletimLoteUe repositorioBoletimLoteUe)
        {
            this.repositorioBoletimLoteUe = repositorioBoletimLoteUe;
        }

        public Task<IEnumerable<BoletimLoteUeRealizaramProvaDto>> Handle(ObterUesAlunosRealizaramProvaPorLoteIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimLoteUe.ObterUesAlunosRealizaramProvaPorLoteId(request.LoteId);
        }
    }
}
