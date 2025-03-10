using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.BoletimLoteProva.DesativarTodos;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvasFinalizadasPorData;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterUltimoBoletimLoteId;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class BuscarProvasFinalizadasUseCase : AbstractUseCase, IBuscarProvasFinalizadasUseCase
    {
        private readonly IServicoLog servicoLog;
        public BuscarProvasFinalizadasUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog) : base(mediator, channel)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var dataProvasFinalizadas = DateTime.Now.AddDays(-1).Date;
                var provasFinalizadas = await mediator.Send(new ObterProvasFinalizadasPorDataQuery(dataProvasFinalizadas));
                if (provasFinalizadas?.Any() ?? false)
                {
                    await DesativarBoletimLotes();
                    var novoLoteId = await ObterLoteId();

                    foreach (var provaFinalizada in provasFinalizadas)
                    {
                        var boletimLoteProva = ObterBoletimLoteProva(provaFinalizada, novoLoteId);
                        await mediator.Send(new InserirBoletimLoteProvaCommand(boletimLoteProva));

                        provaFinalizada.LoteId = boletimLoteProva.LoteId;
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.BuscarAlunosProvaProficienciaBoletim, provaFinalizada));
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

        private static BoletimLoteProva ObterBoletimLoteProva(ProvaDto provaFinalizada, long loteId)
        {
            return new BoletimLoteProva(provaFinalizada.Id, loteId, true);
        }

        private async Task<long> ObterLoteId()
        {
            var ultimoLoteId = await mediator.Send(new ObterUltimoBoletimLoteIdQuery());
            return ultimoLoteId + 1;
        }

        private async Task DesativarBoletimLotes()
        {
            await mediator.Send(new DesativarTodosBoletimLotesCommand());
        }
    }
}
