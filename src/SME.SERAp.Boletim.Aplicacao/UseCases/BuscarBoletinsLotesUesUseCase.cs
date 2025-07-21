using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Exceptions;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class BuscarBoletinsLotesUesUseCase : AbstractUseCase, IBuscarBoletinsLotesUesUseCase
    {
        private readonly IServicoLog servicoLog;
        public BuscarBoletinsLotesUesUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog) : base(mediator, channel)
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

                var boletimLoteUes = await mediator.Send(new ObterUesTotalAlunosPorLoteIdQuery(loteId));
                if (boletimLoteUes?.Any() ?? false)
                {
                    var uesAlunosRealizaramProva = await mediator.Send(new ObterUesAlunosRealizaramProvaPorLoteIdQuery(loteId));

                    foreach (var boletimLoteUe in boletimLoteUes)
                    {
                        AtualizarAlunosRealizaramPova(uesAlunosRealizaramProva, boletimLoteUe);
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarBoletimLoteUe, boletimLoteUe));
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

        private static void AtualizarAlunosRealizaramPova(IEnumerable<BoletimLoteUeRealizaramProvaDto> uesAlunosRealizaramProva, BoletimLoteUe boletimLoteUe)
        {
            boletimLoteUe.RealizaramProva = uesAlunosRealizaramProva
                                        .FirstOrDefault(x => x.LoteId == boletimLoteUe.LoteId && x.UeId == boletimLoteUe.UeId && x.AnoEscolar == boletimLoteUe.AnoEscolar)?.RealizaramProva ?? 0;

            if (boletimLoteUe.RealizaramProva > boletimLoteUe.TotalAlunos)
                boletimLoteUe.TotalAlunos = boletimLoteUe.RealizaramProva;
        }
    }
}
