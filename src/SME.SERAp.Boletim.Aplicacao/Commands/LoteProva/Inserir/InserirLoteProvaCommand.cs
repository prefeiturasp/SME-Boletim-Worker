using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class InserirLoteProvaCommand : IRequest<long>
    {
        public InserirLoteProvaCommand(LoteProva loteProva)
        {
            LoteProva = loteProva;
        }

        public LoteProva LoteProva { get;set; }
    }
}
