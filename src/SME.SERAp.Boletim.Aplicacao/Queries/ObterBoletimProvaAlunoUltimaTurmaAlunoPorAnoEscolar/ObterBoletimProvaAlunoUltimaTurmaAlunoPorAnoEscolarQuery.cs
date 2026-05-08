using MediatR;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar
{
    public class ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery : IRequest<BoletimProvaAlunoUltimaTurmaAlunoDto>
    {
        public long AlunRa { get; set; }
        public int AnoLetivo { get; set; }
        public int AnoEscolar { get; set; }
        public long DisciplinaId { get; set; }
        public decimal Proficiencia { get; set; }
        public ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery(long alunRa, int anoLetivo, int anoEscolar, long disciplinaId, decimal proficiencia)
        {
            AlunRa = alunRa;
            AnoLetivo = anoLetivo;
            AnoEscolar = anoEscolar;
            DisciplinaId = disciplinaId;
            Proficiencia = proficiencia;
        }
    }
}
