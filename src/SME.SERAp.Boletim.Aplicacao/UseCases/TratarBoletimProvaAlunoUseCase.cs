using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolar;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterQuantidadeMensagensPorNomeFila;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class TratarBoletimProvaAlunoUseCase : AbstractUseCase, ITratarBoletimProvaAlunoUseCase
    {
        private readonly IServicoLog servicoLog;
        private readonly IConsolidarBoletimEscolarLoteUseCase consolidarBoletimEscolarUseCase;
        private readonly RabbitOptions rabbitOptions;

        public TratarBoletimProvaAlunoUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog, IConsolidarBoletimEscolarLoteUseCase consolidarBoletimEscolarUseCase, RabbitOptions rabbitOptions) : base(mediator, channel)
        {
            this.servicoLog = servicoLog;
            this.consolidarBoletimEscolarUseCase = consolidarBoletimEscolarUseCase;
            this.rabbitOptions = rabbitOptions;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var alunoProvaProficienciaBoletimDto = mensagemRabbit.ObterObjetoMensagem<AlunoProvaProficienciaBoletimDto>();
                if (alunoProvaProficienciaBoletimDto is null) return true;

                var boletimAlunoProvaExistentes = await mediator
                    .Send(new ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQuery(alunoProvaProficienciaBoletimDto.ProvaId,
                        alunoProvaProficienciaBoletimDto.CodigoAluno, alunoProvaProficienciaBoletimDto.AnoEscolar));

                if (boletimAlunoProvaExistentes?.Any() ?? false)
                {
                    foreach (var boletimAlunoProvaExistente in boletimAlunoProvaExistentes)
                    {
                        await mediator.Send(new ExcluirBoletimProvaAlunoCommand(boletimAlunoProvaExistente.Id));
                    }
                }

                var boletimProvaAluno = ObterBoletimProvaAluno(alunoProvaProficienciaBoletimDto);
                await mediator.Send(new InserirBoletimProvaAlunoCommand(boletimProvaAluno));

                await ConsolidarBoletim(alunoProvaProficienciaBoletimDto);
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }

            return true;
        }

        private async Task ConsolidarBoletim(AlunoProvaProficienciaBoletimDto alunoProvaProficienciaBoletimDto)
        {
            var quantidadeMensagemFila = await mediator.Send(new ObterQuantidadeMensagensPorNomeFilaQuery(RotasRabbit.TratarBoletimProvaAluno));
            if (quantidadeMensagemFila == 0)
            {
                await consolidarBoletimEscolarUseCase.Executar(alunoProvaProficienciaBoletimDto.BoletimLoteId);
            }
        }

        private static BoletimProvaAluno ObterBoletimProvaAluno(AlunoProvaProficienciaBoletimDto alunoProvaProficienciaBoletimDto)
        {
            return new BoletimProvaAluno(alunoProvaProficienciaBoletimDto.CodigoDre, alunoProvaProficienciaBoletimDto.CodigoUe, alunoProvaProficienciaBoletimDto.NomeUe,
                alunoProvaProficienciaBoletimDto.ProvaId, alunoProvaProficienciaBoletimDto.NomeProva, alunoProvaProficienciaBoletimDto.AnoEscolar, alunoProvaProficienciaBoletimDto.Turma,
                alunoProvaProficienciaBoletimDto.CodigoAluno, alunoProvaProficienciaBoletimDto.NomeAluno, alunoProvaProficienciaBoletimDto.NomeDisciplina, alunoProvaProficienciaBoletimDto.DisciplinaId,
                alunoProvaProficienciaBoletimDto.ProvaStatus, alunoProvaProficienciaBoletimDto.Profeciencia, alunoProvaProficienciaBoletimDto.ErroMedida, alunoProvaProficienciaBoletimDto.NivelCodigo);
        }
    }
}
