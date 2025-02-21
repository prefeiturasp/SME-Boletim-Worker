using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.SerapEstudantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Commands.Prova.Alterar
{
    public class AlterarProvaCommand : IRequest<long>
    {
        public AlterarProvaCommand(ProvaDto provaDto)
        {
            ProvaDto = provaDto;
        }

        public ProvaDto ProvaDto { get; set; }
    }
}