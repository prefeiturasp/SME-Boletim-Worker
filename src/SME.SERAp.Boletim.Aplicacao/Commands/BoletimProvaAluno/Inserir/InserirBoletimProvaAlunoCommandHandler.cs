using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class InserirBoletimProvaAlunoCommandHandler : IRequestHandler<InserirBoletimProvaAlunoCommand, long>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;

        public InserirBoletimProvaAlunoCommandHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
        }

        public Task<long> Handle(InserirBoletimProvaAlunoCommand request, CancellationToken cancellationToken)
        {
            return repositorioBoletimProvaAluno.IncluirAsync(request.BoletimProvaAluno);
        }
    }
}
