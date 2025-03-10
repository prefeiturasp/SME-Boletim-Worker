using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Commands.BoletimLoteProva.DesativarTodos
{
    public class DesativarTodosBoletimLotesCommandHandler : IRequestHandler<DesativarTodosBoletimLotesCommand, int>
    {
        private readonly IRepositorioBoletimLoteProva repositorioBoletimLoteProva;
        public DesativarTodosBoletimLotesCommandHandler(IRepositorioBoletimLoteProva repositorioBoletimLoteProva)
        {
            this.repositorioBoletimLoteProva = repositorioBoletimLoteProva;
        }

        public Task<int> Handle(DesativarTodosBoletimLotesCommand request, CancellationToken cancellationToken)
        {
            return repositorioBoletimLoteProva.DesativarTodosBoletimLotes();
        }
    }
}
