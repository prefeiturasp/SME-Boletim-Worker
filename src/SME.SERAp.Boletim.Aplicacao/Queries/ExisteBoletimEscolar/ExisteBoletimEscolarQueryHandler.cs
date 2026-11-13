using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ExisteBoletimEscolar
{
    public class ExisteBoletimEscolarQueryHandler : IRequestHandler<ExisteBoletimEscolarQuery, bool>
    {
        private readonly IRepositorioBoletimEscolar repositorioBoletimEscolar;

        public ExisteBoletimEscolarQueryHandler(IRepositorioBoletimEscolar repositorioBoletimEscolar)
        {
            this.repositorioBoletimEscolar = repositorioBoletimEscolar;
        }

        public Task<bool> Handle(ExisteBoletimEscolarQuery request, CancellationToken cancellationToken)
        {
            return repositorioBoletimEscolar.ExisteBoletimEscolarAsync(
                request.UeId, request.ProvaId, request.DisciplinaId, request.ComponenteCurricular);
        }
    }
}