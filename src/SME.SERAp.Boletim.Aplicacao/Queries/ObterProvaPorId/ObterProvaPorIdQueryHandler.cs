using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaPorId
{
    public class ObterProvaPorIdQueryHandler : IRequestHandler<ObterProvaPorIdQuery, Dominio.Entities.Prova>
    {
        private readonly IRepositorioProva repositorioProva;

        public ObterProvaPorIdQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }

        public async Task<Dominio.Entities.Prova> Handle(ObterProvaPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProva.ObterPorIdAsync(request.Id);
        }
    }
}