using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class InserirBoletimEscolarCommandHandler : IRequestHandler<InserirBoletimEscolarCommand, long>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;
        public InserirBoletimEscolarCommandHandler(IRepositorioBoletimEscolar repositorioBoletimEscolar)
        {
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
        }

        public Task<long> Handle(InserirBoletimEscolarCommand request, CancellationToken cancellationToken)
        {
            return repositorioBoletimEscolar.IncluirAsync(request.BoletimEscolar);
        }
    }
}
