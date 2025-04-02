using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class DesativarTodosLotesProvaAtivosCommandHandler : IRequestHandler<DesativarTodosLotesProvaAtivosCommand, int>
    {
        private readonly IRepositorioLoteProva repositorioLoteProva;
        public DesativarTodosLotesProvaAtivosCommandHandler(IRepositorioLoteProva repositorioLoteProva)
        {
            this.repositorioLoteProva = repositorioLoteProva;
        }

        public Task<int> Handle(DesativarTodosLotesProvaAtivosCommand request, CancellationToken cancellationToken)
        {
            return repositorioLoteProva.DesativarTodosLotesProvaAtivos();
        }
    }
}
