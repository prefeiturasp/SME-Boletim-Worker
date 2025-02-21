using MediatR;
using SME.SERAp.Boletim.Infra.Dtos.SerapEstudantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Commands.Prova.Inserir
{
    public class InserirProvaCommand : IRequest<long>
    {
        public InserirProvaCommand(ProvaDto provaDto)
        {
            ProvaDto = provaDto;
        }

        public ProvaDto ProvaDto { get; set; }
    }
}