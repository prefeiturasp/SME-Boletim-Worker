using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.CalcularProbabilidade;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunosBoletimResultadoProbabilidadePorTurmaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaIdAnoEscolar;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterQuestoesBoletimResultaProbabilidadePorProvaId;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Dominio.Enums;
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

                var alunos = await ObterAlunosBoletimResultadoProbabilidadePorTurma(turmaBoletimResultadoProbabilidadeDto);
                if (!alunos?.Any() ?? true)
                    throw new Exception($"Não foram encontrados alunos para a turma {turmaBoletimResultadoProbabilidadeDto.TurmaId}");

                var questoes = await ObterQuestoesBoletimResultaProbabilidadePorProvaId(turmaBoletimResultadoProbabilidadeDto.ProvaId);
                if (!questoes?.Any() ?? true)
                    throw new Exception($"Não foram encontradas questões para a prova {turmaBoletimResultadoProbabilidadeDto.ProvaId}");

                var niveisProficiencia = await mediator.Send(new ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQuery(turmaBoletimResultadoProbabilidadeDto.DisciplinaId, turmaBoletimResultadoProbabilidadeDto.AnoEscolar));
                AtualizarNivelProficienciaAlunos(alunos, niveisProficiencia);

                foreach (var questao in questoes)
                {
                    foreach (var aluno in alunos)
                    {
                        var probabilidadeQuestaoAluno = await mediator.Send(new CalcularProbabilidadeCommand(questao.AcertoCasual, questao.Dificuldade, questao.Discriminacao, aluno.Proficiencia));
                        aluno.AdicionarQuestao(questao.CodigoHabilidade, questao.DescricaoHabilidade,
                            questao.QuestaoLegadoId, probabilidadeQuestaoAluno);
                    }
                }

                var habilidades = questoes.GroupBy(x => x.CodigoHabilidade);
                foreach (var habilidade in habilidades)
                {
                    var habilidadeCodigo = habilidade.Key;
                    var habilidadeId = habilidade.First().HabilidadeId;
                    var habilidadeDescricao = habilidade.First().DescricaoHabilidade;

                    var abaixoBasico = ObterPorcentagemProbabilidadePorNivelProficienciaDaHabilidade(alunos, habilidadeCodigo, (int)TipoNivelProficiencia.AbaixoBasico);
                    var basico = ObterPorcentagemProbabilidadePorNivelProficienciaDaHabilidade(alunos, habilidadeCodigo, (int)TipoNivelProficiencia.Basico);
                    var adequado = ObterPorcentagemProbabilidadePorNivelProficienciaDaHabilidade(alunos, habilidadeCodigo, (int)TipoNivelProficiencia.Adequado);
                    var avancado = ObterPorcentagemProbabilidadePorNivelProficienciaDaHabilidade(alunos, habilidadeCodigo, (int)TipoNivelProficiencia.Avancado);

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

        private static void AtualizarNivelProficienciaAlunos(IEnumerable<AlunoBoletimResultadoProbabilidadeDto> alunos, IEnumerable<NivelProficiencia> niveisProficiencia)
        {
            foreach (var aluno in alunos)
                aluno.NivelProficiencia = ObterNivelProficiencia(niveisProficiencia, aluno.Proficiencia);
        }

        private async Task<IEnumerable<QuestaoProvaDto>> ObterQuestoesBoletimResultaProbabilidadePorProvaId(long provaId)
        {
            return await mediator.Send(new ObterQuestoesBoletimResultaProbabilidadePorProvaIdQuery(provaId));
        }

        private async Task<IEnumerable<AlunoBoletimResultadoProbabilidadeDto>> ObterAlunosBoletimResultadoProbabilidadePorTurma(TurmaBoletimResultadoProbabilidadeDto turmaBoletimResultadoProbabilidadeDto)
        {
            return await mediator
                                .Send(new ObterAlunosBoletimResultadoProbabilidadePorTurmaIdQuery(turmaBoletimResultadoProbabilidadeDto.TurmaId, turmaBoletimResultadoProbabilidadeDto.ProvaId));
        }

        private static double ObterPorcentagemProbabilidadePorNivelProficienciaDaHabilidade(IEnumerable<AlunoBoletimResultadoProbabilidadeDto> alunos, string habilidadeCodigo, int nivelProficiencia)
        {
            var alunosNivelProficiencia = alunos.Where(x => x.NivelProficiencia == nivelProficiencia);
            if (alunosNivelProficiencia.Any())
            {
                var questoesNivelProficiencia = alunosNivelProficiencia.SelectMany(x => x.Questoes.Where(x => x.CodigoHabilidade == habilidadeCodigo));
                var probabilidadeNivelProficiencia = questoesNivelProficiencia.Sum(x => x.Probabilidade) / questoesNivelProficiencia.Count();
                return probabilidadeNivelProficiencia * 100;
            }

            return 0;
        }

        private static BoletimResultadoProbabilidadeDto ObterBoletimResultadoProbabilidadeTurma(TurmaBoletimResultadoProbabilidadeDto turmaBoletimResultadoProbabilidadeDto, string habilidadeCodigo, long habilidadeId, string habilidadeDescricao, double abaixoBasico, double basico, double adequado, double avancado)
        {
            return new BoletimResultadoProbabilidadeDto(habilidadeId, habilidadeCodigo, habilidadeDescricao, turmaBoletimResultadoProbabilidadeDto.TurmaNome,
                                    turmaBoletimResultadoProbabilidadeDto.TurmaId, turmaBoletimResultadoProbabilidadeDto.ProvaId, turmaBoletimResultadoProbabilidadeDto.UeId, turmaBoletimResultadoProbabilidadeDto.DisciplinaId,
                                    turmaBoletimResultadoProbabilidadeDto.AnoEscolar, abaixoBasico, basico, adequado, avancado);
        }

        private static int ObterNivelProficiencia(IEnumerable<NivelProficiencia> nivelProficiencias, double proficiencia)
        {
            var nivelProficiencia = nivelProficiencias.OrderBy(x => x.Codigo)
                .FirstOrDefault(x => proficiencia < x.ValorReferencia || x.ValorReferencia is null);

            return nivelProficiencia?.Codigo ?? 0;
        }
    }
}