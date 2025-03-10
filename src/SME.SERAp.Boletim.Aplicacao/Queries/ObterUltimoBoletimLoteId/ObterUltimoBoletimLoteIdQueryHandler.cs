using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterUltimoBoletimLoteId
{
    public class ObterUltimoBoletimLoteIdQueryHandler 
        : IRequestHandler<ObterUltimoBoletimLoteIdQuery, long>
    {
        private readonly IRepositorioBoletimLoteProva repositorioBoletimLoteProva;
        public ObterUltimoBoletimLoteIdQueryHandler(IRepositorioBoletimLoteProva repositorioBoletimLoteProva)
        {
            this.repositorioBoletimLoteProva = repositorioBoletimLoteProva;
        }

        public Task<long> Handle(ObterUltimoBoletimLoteIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimLoteProva.ObterUltimoBoletimLoteId();
        }
    }
}
