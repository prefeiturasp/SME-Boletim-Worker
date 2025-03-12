using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolar
{
    public class ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQuery : IRequest<IEnumerable<BoletimProvaAluno>>
    {
        public ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQuery(long provaId, long alunoRa, int anoEscolar)
        {
            ProvaId = provaId;
            AlunoRa = alunoRa;
            AnoEscolar = anoEscolar;
        }

        public long ProvaId { get; set; }
        public long AlunoRa { get; set; }
        public int AnoEscolar { get; set; }
    }
}
