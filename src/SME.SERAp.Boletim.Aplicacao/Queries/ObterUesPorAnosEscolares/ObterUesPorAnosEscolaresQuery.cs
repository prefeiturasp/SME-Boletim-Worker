using MediatR;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterUesPorAnosEscolaresQuery : IRequest<IEnumerable<UeDto>>
    {
        public IEnumerable<string> AnosEscolares { get; set; }
        public int AnoLetivo { get; set; }
        public ObterUesPorAnosEscolaresQuery(IEnumerable<string> anosEscolares, int anoLetivo)
        {
            AnosEscolares = anosEscolares;
            AnoLetivo = anoLetivo;
        }
    }
}
