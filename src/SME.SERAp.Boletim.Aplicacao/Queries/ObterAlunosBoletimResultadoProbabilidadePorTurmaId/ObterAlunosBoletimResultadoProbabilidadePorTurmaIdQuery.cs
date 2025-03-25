using MediatR;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunosBoletimResultadoProbabilidadePorTurmaId
{
    public class ObterAlunosBoletimResultadoProbabilidadePorTurmaIdQuery : IRequest<IEnumerable<AlunoBoletimResultadoProbabilidadeDto>>
    {
        public ObterAlunosBoletimResultadoProbabilidadePorTurmaIdQuery(long turmaId, long provaId)
        {
            TurmaId = turmaId;
            ProvaId = provaId;
        }

        public long TurmaId { get; set; }

        public long ProvaId { get; set; }
    }
}
