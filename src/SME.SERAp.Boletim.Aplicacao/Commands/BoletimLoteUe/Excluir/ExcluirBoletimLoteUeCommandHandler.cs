using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Commands.BoletimLoteUe.Excluir
{
    public class ExcluirBoletimLoteUeCommandHandler : IRequestHandler<ExcluirBoletimLoteUeCommand, int>
    {
        private readonly IRepositorioBoletimLoteUe repositorioBoletimLoteUe;
        public ExcluirBoletimLoteUeCommandHandler(IRepositorioBoletimLoteUe repositorioBoletimLoteUe)
        {
            this.repositorioBoletimLoteUe = repositorioBoletimLoteUe;
        }

        public Task<int> Handle(ExcluirBoletimLoteUeCommand request, CancellationToken cancellationToken)
        {
            return repositorioBoletimLoteUe.ExcluirBoletimLoteUe(request.LoteId, request.UeId, request.AnoEscolar);
        }
    }
}
