using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimLoteProvaPorLoteId;
using SME.SERAp.Boletim.Infra.Exceptions;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class BuscarProvasBoletimLoteUseCase : AbstractUseCase, IBuscarProvasBoletimLoteUseCase
    {
        private readonly IServicoLog servicoLog;
        public BuscarProvasBoletimLoteUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog) : base(mediator, channel)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var loteId = long.Parse(mensagemRabbit.Mensagem.ToString() ?? string.Empty);
                if (loteId == 0)
                    throw new NegocioException("O Id do lote deve ser informado.");

                var boletimLoteProvas = await mediator.Send(new ObterBoletimLoteProvaPorLoteIdQuery(loteId));
                if (boletimLoteProvas?.Any() ?? false)
                {
                    foreach (var boletimLoteProva in boletimLoteProvas)
                    {
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.BuscarBoletimEscolarProva, boletimLoteProva.ProvaId));
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.BuscarTurmasBoletimResultadoProbabilidadeProva, boletimLoteProva.ProvaId));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }

        }
    }
}
