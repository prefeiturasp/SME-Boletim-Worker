using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterLotesProvaPorData
{
    public class ObterLotesProvaPorDataQueryHandler :
        IRequestHandler<ObterLotesProvaPorDataQuery, IEnumerable<LoteProva>>
    {
        private readonly IRepositorioLoteProva repositorioLoteProva;
        public ObterLotesProvaPorDataQueryHandler(IRepositorioLoteProva repositorioLoteProva)
        {
            this.repositorioLoteProva = repositorioLoteProva;
        }

        public Task<IEnumerable<LoteProva>> Handle(ObterLotesProvaPorDataQuery request, CancellationToken cancellationToken)
        {
            return repositorioLoteProva.ObterLotesProvaPorData(request.Inicio, request.Fim, request.FormatoTai);
        }
    }
}
