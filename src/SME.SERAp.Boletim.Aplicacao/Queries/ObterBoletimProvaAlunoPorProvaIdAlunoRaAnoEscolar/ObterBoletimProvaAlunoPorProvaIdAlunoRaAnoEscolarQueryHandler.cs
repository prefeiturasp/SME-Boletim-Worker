using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolar
{
    public class ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQueryHandler : IRequestHandler<ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQuery, IEnumerable<BoletimProvaAluno>>
    {
        private readonly IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno;
        public ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQueryHandler(IRepositorioBoletimProvaAluno repositorioBoletimProvaAluno)
        {
            this.repositorioBoletimProvaAluno = repositorioBoletimProvaAluno;
        }

        public Task<IEnumerable<BoletimProvaAluno>> Handle(ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimProvaAluno.ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolar(request.ProvaId, request.AlunoRa, request.AnoEscolar);
        }
    }
}
