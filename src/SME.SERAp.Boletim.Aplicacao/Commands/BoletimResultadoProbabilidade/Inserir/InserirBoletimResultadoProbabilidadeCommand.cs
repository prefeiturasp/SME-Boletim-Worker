using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class InserirBoletimResultadoProbabilidadeCommand : IRequest<long>
    {
        public InserirBoletimResultadoProbabilidadeCommand(BoletimResultadoProbabilidade boletimResultadoProbabilidade)
        {
            BoletimResultadoProbabilidade = boletimResultadoProbabilidade;
        }

        public BoletimResultadoProbabilidade BoletimResultadoProbabilidade { get; set; }
    }
}
