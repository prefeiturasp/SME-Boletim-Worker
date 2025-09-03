using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterAlunoProvaSpProficienciaQuery : IRequest<AlunoProvaSpProficiencia>
    {
        public int AnoLetivo { get; set; }
        public long DisciplinaId { get; set; }
        public long AlunoRa { get; set; }
        public ObterAlunoProvaSpProficienciaQuery(int anoLetivo, long disciplinaId, long alunoRa)
        {
            AnoLetivo = anoLetivo;
            DisciplinaId = disciplinaId;
            AlunoRa = alunoRa;
        }
    }
}
