using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicarFilaRabbitSerapEstudante;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolar;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaAnoOriginal;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterQuantidadeMensagensPorNomeFila;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class TratarBoletimProvaAlunoUseCase : AbstractUseCase, ITratarBoletimProvaAlunoUseCase
    {
        private readonly IServicoLog servicoLog;
        private readonly IConsolidarBoletimEscolarLoteUseCase consolidarBoletimEscolarUseCase;

        public TratarBoletimProvaAlunoUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog, IConsolidarBoletimEscolarLoteUseCase consolidarBoletimEscolarUseCase) : base(mediator, channel)
        {
            this.servicoLog = servicoLog;
            this.consolidarBoletimEscolarUseCase = consolidarBoletimEscolarUseCase;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var alunoProvaProficienciaBoletimDto = mensagemRabbit.ObterObjetoMensagem<AlunoProvaProficienciaBoletimDto>();
                if (alunoProvaProficienciaBoletimDto is null) return true;

                await RemoverRegistrosExistentes(alunoProvaProficienciaBoletimDto);

                var provaAnoOriginal = await mediator.Send(new ObterProvaAnoOriginalQuery(alunoProvaProficienciaBoletimDto.ProvaId));
                if (!string.IsNullOrWhiteSpace(provaAnoOriginal))
                {
                    if (int.TryParse(provaAnoOriginal, out var provaAnoEscolar) && provaAnoEscolar != alunoProvaProficienciaBoletimDto.AnoEscolar)
                    {
                        var ano = (await mediator.Send(new ObterAnoProvaQuery(alunoProvaProficienciaBoletimDto.ProvaId))) ?? DateTime.Now.Year;
                        var boletimProvaAlunoUltimaTurmaAluno = await mediator.Send(new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery(alunoProvaProficienciaBoletimDto.CodigoAluno,
                            ano, provaAnoEscolar, alunoProvaProficienciaBoletimDto.DisciplinaId, alunoProvaProficienciaBoletimDto.Proficiencia));

                        if (boletimProvaAlunoUltimaTurmaAluno != null)
                        {
                            alunoProvaProficienciaBoletimDto.CodigoDre = boletimProvaAlunoUltimaTurmaAluno.CodigoDre;
                            alunoProvaProficienciaBoletimDto.CodigoUe = boletimProvaAlunoUltimaTurmaAluno.CodigoUe;
                            alunoProvaProficienciaBoletimDto.NomeUe = boletimProvaAlunoUltimaTurmaAluno.NomeUe;
                            alunoProvaProficienciaBoletimDto.AnoEscolar = boletimProvaAlunoUltimaTurmaAluno.AnoEscolar;
                            alunoProvaProficienciaBoletimDto.Turma = boletimProvaAlunoUltimaTurmaAluno.Turma;
                            alunoProvaProficienciaBoletimDto.NivelCodigo = boletimProvaAlunoUltimaTurmaAluno.NivelCodigo;

                            await RemoverRegistrosExistentes(alunoProvaProficienciaBoletimDto);
                        }
                    }
                }

                var boletimProvaAluno = ObterBoletimProvaAluno(alunoProvaProficienciaBoletimDto);

                await mediator.Send(new InserirBoletimProvaAlunoCommand(boletimProvaAluno));

                await BuscarAlunoProvaSpProficiencia(boletimProvaAluno);

                await ConsolidarBoletim(alunoProvaProficienciaBoletimDto);
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }

            return true;
        }

        private async Task RemoverRegistrosExistentes(AlunoProvaProficienciaBoletimDto alunoProvaProficienciaBoletimDto)
        {
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
        }

        private async Task BuscarAlunoProvaSpProficiencia(BoletimProvaAluno boletimProvaAluno)
        {
            await mediator.Send(new PublicarFilaRabbitSerapEstudanteCommand(RotasRabbit.BuscarAlunoProvaSpProficiencia, boletimProvaAluno));
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
                alunoProvaProficienciaBoletimDto.ProvaStatus, alunoProvaProficienciaBoletimDto.Proficiencia, alunoProvaProficienciaBoletimDto.ErroMedida, alunoProvaProficienciaBoletimDto.NivelCodigo);
        }
    }
}
