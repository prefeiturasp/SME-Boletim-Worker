using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterLotesProvaPorData
{
    public class ObterLotesProvaPorDataQuery : IRequest<IEnumerable<LoteProva>>
    {
        public ObterLotesProvaPorDataQuery(DateTime inicio, DateTime fim, bool formatoTai)
        {
            Inicio = inicio;
            Fim = fim;
            FormatoTai = formatoTai;
        }

        public DateTime Inicio { get; set; }

        public DateTime Fim { get; set; }

        public bool FormatoTai { get; set; }
    }
}
