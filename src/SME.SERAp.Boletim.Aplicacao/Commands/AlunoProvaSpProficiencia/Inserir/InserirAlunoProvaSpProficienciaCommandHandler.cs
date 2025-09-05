using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Commands
{
    public class InserirAlunoProvaSpProficienciaCommandHandler : IRequestHandler<InserirAlunoProvaSpProficienciaCommand, long>
    {
        private readonly IRepositorioAlunoProvaSpProficiencia repositorioAlunoProvaSpProficiencia;
        public InserirAlunoProvaSpProficienciaCommandHandler(IRepositorioAlunoProvaSpProficiencia repositorioAlunoProvaSpProficiencia)
        {
            this.repositorioAlunoProvaSpProficiencia = repositorioAlunoProvaSpProficiencia;
        }

        public Task<long> Handle(InserirAlunoProvaSpProficienciaCommand request, CancellationToken cancellationToken)
        {
            return repositorioAlunoProvaSpProficiencia.IncluirAsync(request.AlunoProvaSpProficiencia);
        }
    }
}
