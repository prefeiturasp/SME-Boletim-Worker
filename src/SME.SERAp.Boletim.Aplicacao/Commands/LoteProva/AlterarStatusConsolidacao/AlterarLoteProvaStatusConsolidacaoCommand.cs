using MediatR;
using SME.SERAp.Boletim.Dominio.Enums;

namespace SME.SERAp.Boletim.Aplicacao.Commands.LoteProva.AlterarStatusConsolidacao
{
    public class AlterarLoteProvaStatusConsolidacaoCommand : IRequest<int>
    {
        public long IdLoteProva { get; set; }

        public LoteStatusConsolidacao StatusConsolidacao { get; set; }

        public AlterarLoteProvaStatusConsolidacaoCommand(long idLoteProva, LoteStatusConsolidacao statusConsolidacao)
        {
            IdLoteProva = idLoteProva;
            StatusConsolidacao = statusConsolidacao;
        }
    }
}
