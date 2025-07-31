using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using SME.SERAp.Boletim.Aplicacao.Commands.CalcularProbabilidade;

namespace SME.SERAp.Boletim.Dominio.Teste.Commands
{
    public class CalcularProbabilidadeCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Calcular_Probabilidade_Corretamente()
        {
            var acertoCasual = 0.25;
            var dificuldade = 0.3;
            var discriminacao = 1.5;
            var proficiencia = 0.7;

            var command = new CalcularProbabilidadeCommand(acertoCasual, dificuldade, discriminacao, proficiencia);
            var handler = new CalcularProbabilidadeCommandHandler();

            var resultado = await handler.Handle(command, CancellationToken.None);

            var esperado = acertoCasual + ((1 - acertoCasual) * (1 / (1 + Math.Exp(-1.7 * discriminacao * (proficiencia - dificuldade)))));

            Assert.Equal(esperado, resultado, 5);
        }
    }
}
