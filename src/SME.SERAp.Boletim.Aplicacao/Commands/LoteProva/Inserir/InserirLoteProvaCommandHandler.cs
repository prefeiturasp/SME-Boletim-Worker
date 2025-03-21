using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class InserirLoteProvaCommandHandler : IRequestHandler<InserirLoteProvaCommand, long>
    {
        private readonly IRepositorioLoteProva repositorioLoteProva;
        public InserirLoteProvaCommandHandler(IRepositorioLoteProva repositorioLoteProva)
        {
            this.repositorioLoteProva = repositorioLoteProva;
        }

        public Task<long> Handle(InserirLoteProvaCommand request, CancellationToken cancellationToken)
        {
            return repositorioLoteProva.IncluirAsync(request.LoteProva);
        }
    }
}
