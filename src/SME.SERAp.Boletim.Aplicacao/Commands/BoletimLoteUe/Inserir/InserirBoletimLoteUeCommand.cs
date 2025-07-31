using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class InserirBoletimLoteUeCommand : IRequest<long>
    {
        public BoletimLoteUe BoletimLoteUe { get; set; }

        public InserirBoletimLoteUeCommand(BoletimLoteUe boletimLoteUe)
        {
            BoletimLoteUe = boletimLoteUe;
        }
    }
}
