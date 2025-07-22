using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterUesTotalAlunosPorLoteIdQueryHandler : IRequestHandler<ObterUesTotalAlunosPorLoteIdQuery, IEnumerable<BoletimLoteUe>>
    {
        private readonly IRepositorioBoletimLoteUe repositorioBoletimLoteUe;

        public ObterUesTotalAlunosPorLoteIdQueryHandler(IRepositorioBoletimLoteUe repositorioBoletimLoteUe)
        {
            this.repositorioBoletimLoteUe = repositorioBoletimLoteUe;
        }

        public Task<IEnumerable<BoletimLoteUe>> Handle(ObterUesTotalAlunosPorLoteIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimLoteUe.ObterUesTotalAlunosPorLoteId(request.LoteId);
        }
    }
}
