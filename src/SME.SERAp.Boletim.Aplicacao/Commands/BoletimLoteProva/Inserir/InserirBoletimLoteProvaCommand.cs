using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class InserirBoletimLoteProvaCommand : IRequest<long>
    {
        public InserirBoletimLoteProvaCommand(BoletimLoteProva boletimLoteProva)
        {
            BoletimLoteProva = boletimLoteProva;
        }

        public BoletimLoteProva BoletimLoteProva { get; set; }
    }
}
