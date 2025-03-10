using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class InserirBoletimLoteProvaCommandHandler : IRequestHandler<InserirBoletimLoteProvaCommand, long>
    {
        private readonly IRepositorioBoletimLoteProva repositorioBoletimLoteProva;
        public InserirBoletimLoteProvaCommandHandler(IRepositorioBoletimLoteProva repositorioBoletimLoteProva)
        {
            this.repositorioBoletimLoteProva = repositorioBoletimLoteProva;
        }

        public Task<long> Handle(InserirBoletimLoteProvaCommand request, CancellationToken cancellationToken)
        {
            return repositorioBoletimLoteProva.IncluirAsync(request.BoletimLoteProva);
        }
    }
}
