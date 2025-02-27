using MediatR;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunosProvaProficienciaBoletimPorProvaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaPorId;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class BuscaAlunosProvaProficienciaBoletimUseCase : AbstractUseCase, IBuscaAlunosProvaProficienciaBoletimUseCase
    {
        private readonly IServicoLog servicoLog;
        public BuscaAlunosProvaProficienciaBoletimUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
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
                            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TrataBoletimProvaAluno, alunoProvaProficienciaBoletim));
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
