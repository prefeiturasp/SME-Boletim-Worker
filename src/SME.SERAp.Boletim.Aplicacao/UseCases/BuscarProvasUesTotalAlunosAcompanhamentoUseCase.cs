using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Exceptions;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class BuscarProvasUesTotalAlunosAcompanhamentoUseCase : AbstractUseCase, IBuscarProvasUesTotalAlunosAcompanhamentoUseCase
    {
        private readonly IServicoLog servicoLog;
        public BuscarProvasUesTotalAlunosAcompanhamentoUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog) : base(mediator, channel)
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

                var provasLote = await mediator.Send(new ObterProvasTaiPorLoteIdQuery(loteId));
                if (!provasLote?.Any() ?? true)
                    return true;

                var anoLetivo = provasLote.FirstOrDefault().Inicio.Year;
                var anosEscolares = provasLote.Select(x => x.AnoEscolar.ToString()).Distinct().ToList();
                var ues = await mediator.Send(new ObterUesPorAnosEscolaresQuery(anosEscolares, anoLetivo));
                if (!ues?.Any() ?? true)
                    return true;

                foreach (var prova in provasLote)
                {
                    foreach (var ue in ues)
                    {
                        var provaUe = new ProvaUeDto
                        {
                            Id = prova.Id,
                            AnoEscolar = prova.AnoEscolar,
                            DreId = ue.DreId,
                            UeId = ue.Id,
                            LoteId = prova.LoteId
                        };

                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarProvasUesTotalAlunosAcompanhamento, provaUe));
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
