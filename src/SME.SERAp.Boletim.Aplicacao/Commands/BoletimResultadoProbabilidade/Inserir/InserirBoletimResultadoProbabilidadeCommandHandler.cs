using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class InserirBoletimResultadoProbabilidadeCommandHandler
        : IRequestHandler<InserirBoletimResultadoProbabilidadeCommand, long>
    {
        private readonly IRepositorioBoletimResultadoProbabilidade repositorioBoletimResultadoProbabilidade;

        public InserirBoletimResultadoProbabilidadeCommandHandler(IRepositorioBoletimResultadoProbabilidade repositorioBoletimResultadoProbabilidade)
        {
            this.repositorioBoletimResultadoProbabilidade = repositorioBoletimResultadoProbabilidade;
        }

        public Task<long> Handle(InserirBoletimResultadoProbabilidadeCommand request, CancellationToken cancellationToken)
        {
            return repositorioBoletimResultadoProbabilidade.IncluirAsync(request.BoletimResultadoProbabilidade);
        }
    }
}
