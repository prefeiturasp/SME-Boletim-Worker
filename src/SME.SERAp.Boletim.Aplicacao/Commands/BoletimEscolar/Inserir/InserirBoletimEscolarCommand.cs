using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class InserirBoletimEscolarCommand : IRequest<long>
    {
        public InserirBoletimEscolarCommand(BoletimEscolar boletimEscolar)
        {
            BoletimEscolar = boletimEscolar;
        }

        public BoletimEscolar BoletimEscolar { get; set; }
    }
}
