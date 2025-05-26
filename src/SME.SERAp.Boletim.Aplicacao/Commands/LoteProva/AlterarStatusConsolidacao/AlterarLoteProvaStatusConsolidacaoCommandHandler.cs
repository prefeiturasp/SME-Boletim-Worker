using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Commands.LoteProva.AlterarStatusConsolidacao
{
    public class AlterarLoteProvaStatusConsolidacaoCommandHandler : IRequestHandler<AlterarLoteProvaStatusConsolidacaoCommand, int>
    {
        private readonly IRepositorioLoteProva repositorioLoteProva;
        public AlterarLoteProvaStatusConsolidacaoCommandHandler(IRepositorioLoteProva repositorioLoteProva)
        {
            this.repositorioLoteProva = repositorioLoteProva;
        }

        public async Task<int> Handle(AlterarLoteProvaStatusConsolidacaoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioLoteProva.AlterarStatusConsolidacao(request.IdLoteProva, request.StatusConsolidacao);
        }
    }
}
