using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaIdAnoEscolar
{
    public class ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQuery : IRequest<IEnumerable<NivelProficiencia>>
    {
        public ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQuery(long disciplinaId, long anoEscolar)
        {
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
        }

        public long DisciplinaId { get; set; }

        public long AnoEscolar { get; set; }
    }
}
