using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunosBoletimResultadoProbabilidadePorTurmaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaIdAnoEscolar;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterQuestoesBoletimResultaProbabilidadePorProvaId;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class TratarTurmaBoletimResultadoProbabilidadeProvaUseCase : AbstractUseCase, ITratarTurmaBoletimResultadoProbabilidadeProvaUseCase
    {
        private readonly IServicoLog servicoLog;

        public TratarTurmaBoletimResultadoProbabilidadeProvaUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog) : base(mediator, channel)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var turmaBoletimResultadoProbabilidadeDto = mensagemRabbit.ObterObjetoMensagem<TurmaBoletimResultadoProbabilidadeDto>();
                if (turmaBoletimResultadoProbabilidadeDto is null) return true;

                var alunos = await mediator
                    .Send(new ObterAlunosBoletimResultadoProbabilidadePorTurmaIdQuery(turmaBoletimResultadoProbabilidadeDto.TurmaId, turmaBoletimResultadoProbabilidadeDto.ProvaId));
                if (alunos?.Any() ?? true)
                    throw new Exception($"Não foram encontrados alunos para a turma {turmaBoletimResultadoProbabilidadeDto.TurmaId}");

                var questoes = await mediator.Send(new ObterQuestoesBoletimResultaProbabilidadePorProvaIdQuery(turmaBoletimResultadoProbabilidadeDto.ProvaId));
                if (questoes?.Any() ?? true)
                    throw new Exception($"Não foram encontradas questões para a prova {turmaBoletimResultadoProbabilidadeDto.ProvaId}");

                var niveisProficiencia = await mediator.Send(new ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQuery(turmaBoletimResultadoProbabilidadeDto.DisciplinaId, turmaBoletimResultadoProbabilidadeDto.AnoEscolar));

                foreach (var questao in questoes)
                {
                    foreach (var aluno in alunos)
                    {
                        //var probabilidadeQuestao = mediator.Send(new CalcularProbabilidadeDoAlunoCommand(questao.AcertoCasual, questao.Dificuldade, questao.Discriminacao, aluno.Proficiencia));
                        var probabilidadeQuestao = 0;
                        aluno.AdicionarQuestao(questao.CodigoHabilidade, questao.DescricaoHabilidade, 
                            questao.QuestaoLegadoId, probabilidadeQuestao);
                    }
                }

                var habilidades = questoes.GroupBy(x => x.CodigoHabilidade);
                foreach(var habilidade in habilidades)
                {
                    var habilidadeCodigo = habilidade.Key;
                    var habilidadeId = habilidade.First().HabilidadeId;
                    var habilidadeDescricao = habilidade.First().DescricaoHabilidade;

                    var abaixoBasico = 0M;
                    var basico = 0M;
                    var adequado = 0M;
                    var avancado = 0M;

                    var boletimResultadoProbabilidadeTurmaDto = ObterBoletimResultadoProbabilidadeTurma(turmaBoletimResultadoProbabilidadeDto, habilidadeCodigo, habilidadeId, habilidadeDescricao, abaixoBasico, basico, adequado, avancado);
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarBoletimResultadoProbabilidadeProva, boletimResultadoProbabilidadeTurmaDto));
                }
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }

            return true;
        }

        private static BoletimResultadoProbabilidadeDto ObterBoletimResultadoProbabilidadeTurma(TurmaBoletimResultadoProbabilidadeDto turmaBoletimResultadoProbabilidadeDto, string habilidadeCodigo, long habilidadeId, string habilidadeDescricao, decimal abaixoBasico, decimal basico, decimal adequado, decimal avancado)
        {
            return new BoletimResultadoProbabilidadeDto(habilidadeId, habilidadeCodigo, habilidadeDescricao, turmaBoletimResultadoProbabilidadeDto.TurmaNome,
                                    turmaBoletimResultadoProbabilidadeDto.TurmaId, turmaBoletimResultadoProbabilidadeDto.ProvaId, turmaBoletimResultadoProbabilidadeDto.UeId, turmaBoletimResultadoProbabilidadeDto.DisciplinaId,
                                    turmaBoletimResultadoProbabilidadeDto.AnoEscolar, abaixoBasico, basico, adequado, avancado);
        }

        private static int ObterNivelProficiencia(IEnumerable<NivelProficiencia> nivelProficiencias, decimal proficiencia)
        {
            var nivelProficiencia = nivelProficiencias.OrderBy(x => x.Codigo)
                .FirstOrDefault(x => proficiencia < x.ValorReferencia || x.ValorReferencia is null);

            return nivelProficiencia?.Codigo ?? 0;
        }
    }
}