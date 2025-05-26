using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimLoteProvaPendentes
{
    public class ObterBoletimLoteProvaPendentesQueryHandler
        : IRequestHandler<ObterBoletimLoteProvaPendentesQuery, IEnumerable<BoletimLoteProva>>
    {
        private readonly IRepositorioBoletimLoteProva repositorioBoletimLoteProva;
        public ObterBoletimLoteProvaPendentesQueryHandler(IRepositorioBoletimLoteProva repositorioBoletimLoteProva)
        {
            this.repositorioBoletimLoteProva = repositorioBoletimLoteProva;
        }

        public Task<IEnumerable<BoletimLoteProva>> Handle(ObterBoletimLoteProvaPendentesQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimLoteProva.ObterBoletimLoteProvaPendentes();
        }
    }
}
