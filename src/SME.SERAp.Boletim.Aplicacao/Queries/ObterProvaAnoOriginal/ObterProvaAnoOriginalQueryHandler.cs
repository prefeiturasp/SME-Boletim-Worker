using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaAnoOriginal
{
    public class ObterProvaAnoOriginalQueryHandler : IRequestHandler<ObterProvaAnoOriginalQuery, string>
    {
        private readonly IRepositorioProva repositorioProva;
        public ObterProvaAnoOriginalQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva;
        }

        public Task<string> Handle(ObterProvaAnoOriginalQuery request, CancellationToken cancellationToken)
        {
            return repositorioProva.ObterProvaAnoOriginal(request.ProvaId);
        }
    }
}
