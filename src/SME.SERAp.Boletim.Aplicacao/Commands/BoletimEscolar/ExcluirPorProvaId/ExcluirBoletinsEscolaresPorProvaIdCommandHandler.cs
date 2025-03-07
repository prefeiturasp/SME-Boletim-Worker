using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class ExcluirBoletinsEscolaresPorProvaIdCommandHandler : IRequestHandler<ExcluirBoletinsEscolaresPorProvaIdCommand, int>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;
        public ExcluirBoletinsEscolaresPorProvaIdCommandHandler(IRepositorioBoletimEscolar repositorioBoletimEscolar)
        {
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
        }

        public Task<int> Handle(ExcluirBoletinsEscolaresPorProvaIdCommand request, CancellationToken cancellationToken)
        {
            return repositorioBoletimEscolar.ExcluirBoletinsEscolaresPorProvaIdAsync(request.ProvaId);
        }
    }
}
