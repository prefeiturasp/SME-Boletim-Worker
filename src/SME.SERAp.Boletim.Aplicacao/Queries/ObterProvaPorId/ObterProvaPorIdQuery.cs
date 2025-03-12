using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaPorId
{
    public class ObterProvaPorIdQuery : IRequest<Prova>
    {
        public ObterProvaPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}