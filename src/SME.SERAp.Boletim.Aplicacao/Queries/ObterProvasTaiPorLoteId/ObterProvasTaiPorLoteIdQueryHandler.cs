using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterProvasTaiPorLoteIdQueryHandler : IRequestHandler<ObterProvasTaiPorLoteIdQuery, IEnumerable<ProvaDto>>
    {
        private readonly IRepositorioLoteProva repositorioLoteProva;
        public ObterProvasTaiPorLoteIdQueryHandler(IRepositorioLoteProva repositorioLoteProva)
        {
            this.repositorioLoteProva = repositorioLoteProva;
        }

        public Task<IEnumerable<ProvaDto>> Handle(ObterProvasTaiPorLoteIdQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioLoteProva.ObterProvasTaiAnoPorLoteId(request.LoteId);
        }
    }
}
