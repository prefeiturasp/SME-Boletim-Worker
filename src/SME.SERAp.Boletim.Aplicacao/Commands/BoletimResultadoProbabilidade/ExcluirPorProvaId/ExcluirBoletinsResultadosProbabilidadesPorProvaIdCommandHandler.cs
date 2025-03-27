using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Commands.BoletimResultadoProbabilidade.ExcluirPorProvaId
{
    public class ExcluirBoletinsResultadosProbabilidadesPorProvaIdCommandHandler
        : IRequestHandler<ExcluirBoletinsResultadosProbabilidadesPorProvaIdCommand, int>
    {
        private readonly IRepositorioBoletimResultadoProbabilidade repositorioBoletimResultadoProbabilidade;
        public ExcluirBoletinsResultadosProbabilidadesPorProvaIdCommandHandler(IRepositorioBoletimResultadoProbabilidade repositorioBoletimResultadoProbabilidade)
        {
            this.repositorioBoletimResultadoProbabilidade = repositorioBoletimResultadoProbabilidade;
        }

        public Task<int> Handle(ExcluirBoletinsResultadosProbabilidadesPorProvaIdCommand request, CancellationToken cancellationToken)
        {
            return repositorioBoletimResultadoProbabilidade.ExcluirBoletinsResultadosProbabilidadesPorProvaIdAsync(request.ProvaId);
        }
    }
}
