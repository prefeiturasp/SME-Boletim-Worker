using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.SerapEstudantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries.SerapEstudantes.ObterTodasProvasSerap
{
    public class ObterTodasProvasSerapQuery : IRequest<IEnumerable<ProvaDto>>
    {
    }
}