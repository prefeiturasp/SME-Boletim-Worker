using MediatR;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class ExcluirBoletinsEscolaresPorProvaIdCommand : IRequest<int>
    {
        public ExcluirBoletinsEscolaresPorProvaIdCommand(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
