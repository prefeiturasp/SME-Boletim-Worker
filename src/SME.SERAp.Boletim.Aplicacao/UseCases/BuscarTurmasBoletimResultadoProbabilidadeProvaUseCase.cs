using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.BoletimResultadoProbabilidade.ExcluirPorProvaId;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaPorId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterTurmasBoletimResultadoProbabilidadePorProvaId;
using SME.SERAp.Boletim.Infra.Exceptions;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class BuscarTurmasBoletimResultadoProbabilidadeProvaUseCase : AbstractUseCase, IBuscarTurmasBoletimResultadoProbabilidadeProvaUseCase
    {
        private readonly IServicoLog servicoLog;
        public BuscarTurmasBoletimResultadoProbabilidadeProvaUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog) : base(mediator, channel)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaId = long.Parse(mensagemRabbit.Mensagem.ToString() ?? string.Empty);
                if (provaId == 0)
                    throw new NegocioException("O Id da prova deve ser informado.");

                var prova = await mediator.Send(new ObterProvaPorIdQuery(provaId));
                if (prova is null)
                    throw new NegocioException($"O prova {provaId} não encontrada.");

                var turmasBoletimResultadoProbabilidadeDtos = await mediator.Send(new ObterTurmasBoletimResultadoProbabilidadePorProvaIdQuery(prova.Id));
                if (turmasBoletimResultadoProbabilidadeDtos?.Any() ?? false)
                {
                    await RemoverBoletinsResultadosProbabilidadesExistentesParaReinsercao(provaId);

                    foreach (var turma in turmasBoletimResultadoProbabilidadeDtos)
                    {
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarTurmaBoletimResultadoProbabilidadeProva, turma));
                    }
                }
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }

            return true;
        }

        private async Task RemoverBoletinsResultadosProbabilidadesExistentesParaReinsercao(long provaId)
        {
            await mediator.Send(new ExcluirBoletinsResultadosProbabilidadesPorProvaIdCommand(provaId));
        }
    }
}