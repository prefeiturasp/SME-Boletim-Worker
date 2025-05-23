using MediatR;
using SME.SERAp.Boletim.Aplicacao.Commands.LoteProva.AlterarStatusConsolidacao;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Dominio.Enums;
using SME.SERAp.Boletim.Infra.Fila;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class ConsolidarBoletimEscolarLoteUseCase : IConsolidarBoletimEscolarLoteUseCase
    {
        private readonly List<long> _lotesConsolidar;
        private readonly IMediator mediator;
        public ConsolidarBoletimEscolarLoteUseCase(IMediator mediator)
        {
            _lotesConsolidar = new List<long>();
            this.mediator = mediator;
        }

        public async Task Executar(long loteId)
        {
            if (_lotesConsolidar.Contains(loteId))
                return;

            _lotesConsolidar.Add(loteId);
            await mediator.Send(new AlterarLoteProvaStatusConsolidacaoCommand(loteId, LoteStatusConsolidacao.Pendente));

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMinutes(2));
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.BuscarProvasBoletimLote));
                _lotesConsolidar.Remove(loteId);
            });
        }
    }
}

