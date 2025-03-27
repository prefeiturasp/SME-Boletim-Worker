using MediatR;

namespace SME.SERAp.Boletim.Aplicacao.Commands.BoletimResultadoProbabilidade.ExcluirPorProvaId
{
    public class ExcluirBoletinsResultadosProbabilidadesPorProvaIdCommand : IRequest<int>
    {
        public ExcluirBoletinsResultadosProbabilidadesPorProvaIdCommand(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
