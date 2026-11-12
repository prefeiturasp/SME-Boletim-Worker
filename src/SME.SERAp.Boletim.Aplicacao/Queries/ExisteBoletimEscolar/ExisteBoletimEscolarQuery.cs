using MediatR;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ExisteBoletimEscolar
{
    public class ExisteBoletimEscolarQuery : IRequest<bool>
    {
        public ExisteBoletimEscolarQuery(long ueId, long provaId, long? disciplinaId, string? componenteCurricular)
        {
            UeId = ueId;
            ProvaId = provaId;
            DisciplinaId = disciplinaId;
            ComponenteCurricular = componenteCurricular;
        }

        public long UeId { get; }
        public long ProvaId { get; }
        public long? DisciplinaId { get; }
        public string? ComponenteCurricular { get; }
    }
}