using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterAnoProvaQueryHandler : IRequestHandler<ObterAnoProvaQuery, int?>
    {
        private readonly IRepositorioProva repositorio;
        public ObterAnoProvaQueryHandler(IRepositorioProva repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<int?> Handle(ObterAnoProvaQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterAnoProva(request.ProvaId);
        }
    }
}
