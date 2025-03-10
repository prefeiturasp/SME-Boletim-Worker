using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunosProvaProficienciaBoletimPorProvaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaPorId;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class BuscarAlunosProvaProficienciaBoletimUseCase : AbstractUseCase, IBuscarAlunosProvaProficienciaBoletimUseCase
    {
        private readonly IServicoLog servicoLog;
        public BuscarAlunosProvaProficienciaBoletimUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog) : base(mediator, channel)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaFinalizadaDto = mensagemRabbit.ObterObjetoMensagem<ProvaDto>();
                if (provaFinalizadaDto == null) return false;

                var prova = await mediator.Send(new ObterProvaPorIdQuery(provaFinalizadaDto.Id));
                if (prova is not null)
                {
                    var alunosProvaProficienciaBoletim = await mediator.Send(new ObterAlunosProvaProficienciaBoletimPorProvaIdQuery(prova.Id));
                    if (alunosProvaProficienciaBoletim?.Any() ?? false)
                    {
                        foreach (var alunoProvaProficienciaBoletim in alunosProvaProficienciaBoletim)
                        {
                            alunoProvaProficienciaBoletim.BoletimLoteId = provaFinalizadaDto.LoteId;
                            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarBoletimProvaAluno, alunoProvaProficienciaBoletim));
                        }
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
    }
}
