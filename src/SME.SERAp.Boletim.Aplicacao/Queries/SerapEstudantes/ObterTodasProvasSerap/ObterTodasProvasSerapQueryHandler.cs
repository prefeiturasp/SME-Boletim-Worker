using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces.SerapEstudantes;
using SME.SERAp.Boletim.Infra.Dtos.SerapEstudantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.SerapEstudantes.ObterTodasProvasSerap
{
    public class ObterTodasProvasSerapQueryHandler : IRequestHandler<ObterTodasProvasSerapQuery, IEnumerable<ProvaDto>>
    {
        private readonly IRepositorioSerapProva repositorioSerapProva;

        public ObterTodasProvasSerapQueryHandler(IRepositorioSerapProva repositorioSerapProva)
        {
            this.repositorioSerapProva = repositorioSerapProva ?? throw new ArgumentNullException(nameof(repositorioSerapProva));
        }

        public async Task<IEnumerable<ProvaDto>> Handle(ObterTodasProvasSerapQuery request, CancellationToken cancellationToken)
        {
            return await repositorioSerapProva.ObterTodosAsync();
        }
    }
}