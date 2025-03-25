using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SME.SERAp.Boletim.Aplicacao.Commands.CalcularProbabilidade
{
    public class CalcularProbabilidadeCommandHandler : IRequestHandler<CalcularProbabilidadeCommand, double>
    {

        public CalcularProbabilidadeCommandHandler()
        {

        }

        public async Task<double> Handle(CalcularProbabilidadeCommand request, CancellationToken cancellationToken)
        {
            var probabilidadeAluno = request.AcertoCasual + ((1 - request.AcertoCasual) * (1 / (1 + Math.Exp(-1.7 * request.Discriminacao * (request.Proficiencia - request.Dificuldade)))));

            return probabilidadeAluno;
        }
    }
}
