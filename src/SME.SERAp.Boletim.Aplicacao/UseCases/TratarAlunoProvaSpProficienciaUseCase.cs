using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class TratarAlunoProvaSpProficienciaUseCase : AbstractUseCase, ITratarAlunoProvaSpProficienciaUseCase
    {
        private readonly IServicoLog servicoLog;
        public TratarAlunoProvaSpProficienciaUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog) : base(mediator, channel)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var alunoProvaSpProficiencia = mensagemRabbit.ObterObjetoMensagem<AlunoProvaSpProficiencia>();
                if (alunoProvaSpProficiencia is null)
                    throw new Exception("Mensagem inválida.");

                var alunoProvaSpProficienciaExistente = await mediator
                    .Send(new ObterAlunoProvaSpProficienciaQuery(alunoProvaSpProficiencia.AnoLetivo, alunoProvaSpProficiencia.DisciplinaId, alunoProvaSpProficiencia.AnoEscolar));

                if(alunoProvaSpProficienciaExistente is not null)
                    await mediator.Send(new ExcluirAlunoProvaSpProficienciaCommand(alunoProvaSpProficiencia.AnoLetivo, alunoProvaSpProficiencia.DisciplinaId, alunoProvaSpProficiencia.AnoEscolar));

                await mediator.Send(new InserirAlunoProvaSpProficienciaCommand(alunoProvaSpProficiencia));
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
