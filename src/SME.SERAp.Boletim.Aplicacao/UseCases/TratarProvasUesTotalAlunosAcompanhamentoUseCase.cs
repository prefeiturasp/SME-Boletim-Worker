using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.Elastic.ObterResumoGeralProvaPorUe;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class TratarProvasUesTotalAlunosAcompanhamentoUseCase : AbstractUseCase, ITratarProvasUesTotalAlunosAcompanhamentoUseCase
    {
        private readonly IServicoLog servicoLog;
        public TratarProvasUesTotalAlunosAcompanhamentoUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog) : base(mediator, channel)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaUeDto = mensagemRabbit.ObterObjetoMensagem<ProvaUeDto>();
                if (provaUeDto is null) return true;

                var provaTurmaAcompanhamento = await mediator.Send(new ObterResumoGeralProvaPorUeQuery(provaUeDto.UeId, provaUeDto.Id, provaUeDto.AnoEscolar));
                if (provaTurmaAcompanhamento is null || provaTurmaAcompanhamento.TotalAlunos == 0) return true;

                var totalRealizaramProva = 0;
                var alunosRealizaramProva = await mediator.Send(new ObterUesAlunosRealizaramProvaQuery(provaUeDto.LoteId, provaUeDto.UeId, provaUeDto.AnoEscolar));
                if (alunosRealizaramProva is not null)
                    totalRealizaramProva = alunosRealizaramProva.RealizaramProva;

                if (totalRealizaramProva > provaTurmaAcompanhamento.TotalAlunos)
                    provaTurmaAcompanhamento.TotalAlunos = totalRealizaramProva;

                var boletimLoteUe = new BoletimLoteUe(provaUeDto.DreId, provaUeDto.UeId, provaUeDto.LoteId, provaUeDto.AnoEscolar, (int)provaTurmaAcompanhamento.TotalAlunos, totalRealizaramProva);
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
