using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimLoteProvaPendentes
{
    public class ObterBoletimLoteProvaPendentesQuery : IRequest<IEnumerable<BoletimLoteProva>>
    {
        public ObterBoletimLoteProvaPendentesQuery()
        {
        }
    }
}
