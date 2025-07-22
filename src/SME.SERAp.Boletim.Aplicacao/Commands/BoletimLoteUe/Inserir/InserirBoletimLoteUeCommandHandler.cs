using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Commands
{
    public class InserirBoletimLoteUeCommandHandler : IRequestHandler<InserirBoletimLoteUeCommand, long>
    {
        private readonly IRepositorioBoletimLoteUe repositorioBoletimLoteUe;
        public InserirBoletimLoteUeCommandHandler(IRepositorioBoletimLoteUe repositorioBoletimLoteUe)
        {
            this.repositorioBoletimLoteUe = repositorioBoletimLoteUe;
        }

        public Task<long> Handle(InserirBoletimLoteUeCommand request, CancellationToken cancellationToken)
        {
            return repositorioBoletimLoteUe.IncluirAsync(request.BoletimLoteUe);
        }
    }
}
