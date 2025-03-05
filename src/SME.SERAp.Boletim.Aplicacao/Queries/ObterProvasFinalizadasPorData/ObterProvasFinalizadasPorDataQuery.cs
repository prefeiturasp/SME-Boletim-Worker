using MediatR;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProvasFinalizadasPorData
{
    public class ObterProvasFinalizadasPorDataQuery : IRequest<IEnumerable<ProvaDto>>
    {
        public ObterProvasFinalizadasPorDataQuery(DateTime data)
        {
            Data = data;
        }

        public DateTime Data { get; set; }
    }
}
