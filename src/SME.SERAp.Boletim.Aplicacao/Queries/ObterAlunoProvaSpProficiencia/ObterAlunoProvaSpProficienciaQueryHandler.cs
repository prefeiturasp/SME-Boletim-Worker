using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterAlunoProvaSpProficienciaQueryHandler : IRequestHandler<ObterAlunoProvaSpProficienciaQuery, AlunoProvaSpProficiencia>
    {
        private readonly IRepositorioAlunoProvaSpProficiencia repositorio;
        public ObterAlunoProvaSpProficienciaQueryHandler(IRepositorioAlunoProvaSpProficiencia repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<AlunoProvaSpProficiencia> Handle(ObterAlunoProvaSpProficienciaQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterAlunoProvaSpProficiencia(request.AnoLetivo, request.DisciplinaId, request.AlunoRa);
        }
    }
}
