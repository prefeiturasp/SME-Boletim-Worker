using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar
{
    public class ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQueryHandler : IRequestHandler<ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery, BoletimProvaAlunoUltimaTurmaAlunoDto>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;
        public ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
        }
        public async Task<BoletimProvaAlunoUltimaTurmaAlunoDto> Handle(ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery request, CancellationToken cancellationToken)
        {
            return await repositorioBoletimProvaAluno.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(request.AlunRa, request.AnoLetivo, request.AnoEscolar, request.DisciplinaId, request.Proficiencia);
        }
    }
}
