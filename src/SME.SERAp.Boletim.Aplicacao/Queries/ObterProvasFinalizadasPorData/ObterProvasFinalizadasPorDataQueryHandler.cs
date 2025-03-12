using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProvasFinalizadasPorData
{
    public class ObterProvasFinalizadasPorDataQueryHandler : IRequestHandler<ObterProvasFinalizadasPorDataQuery, IEnumerable<ProvaDto>>
    {
        private readonly IRepositorioProva repositorioProva;

        public ObterProvasFinalizadasPorDataQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva;
        }

        public Task<IEnumerable<ProvaDto>> Handle(ObterProvasFinalizadasPorDataQuery request, CancellationToken cancellationToken)
        {
            return repositorioProva.ObterProvasFinalizadasPorData(request.Data);
        }
    }
}
