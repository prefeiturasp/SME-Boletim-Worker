using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class TratarBoletimLoteUeUseCase : AbstractUseCase, ITratarBoletimLoteUeUseCase
    {
        private readonly IServicoLog servicoLog;
        public TratarBoletimLoteUeUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog) : base(mediator, channel)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var boletimLoteUe = mensagemRabbit.ObterObjetoMensagem<BoletimLoteUe>();
                if (boletimLoteUe is null) return true;

                await mediator.Send(new ExcluirBoletimLoteUeCommand(boletimLoteUe.LoteId, boletimLoteUe.UeId, boletimLoteUe.AnoEscolar));
                await mediator.Send(new InserirBoletimLoteUeCommand(boletimLoteUe));
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }

            return true;
        }
    }
}
