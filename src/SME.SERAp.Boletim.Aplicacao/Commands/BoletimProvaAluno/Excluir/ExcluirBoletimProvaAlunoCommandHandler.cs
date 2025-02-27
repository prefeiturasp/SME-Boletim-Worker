using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class ExcluirBoletimProvaAlunoCommandHandler : IRequestHandler<ExcluirBoletimProvaAlunoCommand, int>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;

        public ExcluirBoletimProvaAlunoCommandHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
        }

        public Task<int> Handle(ExcluirBoletimProvaAlunoCommand request, CancellationToken cancellationToken)
        {
            return repositorioBoletimProvaAluno.ExcluirPorIdAsync(request.Id);
        }
    }
}
