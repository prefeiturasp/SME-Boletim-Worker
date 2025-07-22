using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.LoteProva.AlterarStatusConsolidacao;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimLoteProvaPendentes;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Dominio.Enums;
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
                var boletimLoteProvas = await mediator.Send(new ObterBoletimLoteProvaPendentesQuery());
                if (boletimLoteProvas?.Any() ?? false)
                {
                    await AlterarStatusLotesProvas(boletimLoteProvas);

                    await GerarBoletimLoteUes(boletimLoteProvas);

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

        private async Task GerarBoletimLoteUes(IEnumerable<BoletimLoteProva> boletimLoteProvas)
        {
            var lotesIds = boletimLoteProvas.Select(x => x.LoteId).Distinct().ToList();
            foreach (var loteId in lotesIds)
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.BuscarBoletimLoteUe, loteId));
        }

        private async Task AlterarStatusLotesProvas(IEnumerable<BoletimLoteProva> boletimLoteProvas)
        {
            var lotesIds = boletimLoteProvas.Select(x => x.LoteId).Distinct().ToList();
            foreach (var loteId in lotesIds)
                await mediator.Send(new AlterarLoteProvaStatusConsolidacaoCommand(loteId, LoteStatusConsolidacao.Consolidado));
        }
    }
}
