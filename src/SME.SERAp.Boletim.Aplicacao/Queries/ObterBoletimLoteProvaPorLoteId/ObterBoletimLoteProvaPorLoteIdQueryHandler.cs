using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimLoteProvaPorLoteId
{
    public class ObterBoletimLoteProvaPorLoteIdQueryHandler
        : IRequestHandler<ObterBoletimLoteProvaPorLoteIdQuery, IEnumerable<BoletimLoteProva>>
    {
        private readonly IRepositorioBoletimLoteProva repositorioBoletimLoteProva;
        public ObterBoletimLoteProvaPorLoteIdQueryHandler(IRepositorioBoletimLoteProva repositorioBoletimLoteProva)
        {
            this.repositorioBoletimLoteProva = repositorioBoletimLoteProva;
        }

        public Task<IEnumerable<BoletimLoteProva>> Handle(ObterBoletimLoteProvaPorLoteIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimLoteProva.ObterBoletimLoteProvaPorLoteId(request.LoteId);
        }
    }
}
