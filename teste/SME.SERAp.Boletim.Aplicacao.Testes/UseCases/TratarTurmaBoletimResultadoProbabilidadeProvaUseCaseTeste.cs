using MediatR;
using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.CalcularProbabilidade;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunosBoletimResultadoProbabilidadePorTurmaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaIdAnoEscolar;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterQuestoesBoletimResultaProbabilidadePorProvaId;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Dominio.Enums;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;
using System.Text.Json;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UseCases
{
    public class TratarTurmaBoletimResultadoProbabilidadeProvaUseCaseTeste
    {
        private readonly TratarTurmaBoletimResultadoProbabilidadeProvaUseCase tratarTurmaBoletimResultadoProbabilidadeProvaUseCase;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IChannel> channel;
        private readonly Mock<IServicoLog> serviceLog;

        public TratarTurmaBoletimResultadoProbabilidadeProvaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            channel = new Mock<IChannel>();
            serviceLog = new Mock<IServicoLog>();
            tratarTurmaBoletimResultadoProbabilidadeProvaUseCase = new TratarTurmaBoletimResultadoProbabilidadeProvaUseCase(mediator.Object, channel.Object, serviceLog.Object);
        }

        [Fact]
        public async Task Deve_Publicar_Fila_Tratar_Boletim_Resultado_Probabilidade_Prova()
        {
            var turmaBoletimResultadoProbabilidadeDto = ObterTurmaBoletimResultadoProbabilidadeDto();
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(turmaBoletimResultadoProbabilidadeDto), Guid.NewGuid());

            var alunosBoletimResultadoProbabilidadeDto = ObterAlunosBoletimResultadoProbabilidadeDto();
            mediator.Setup(m => m.Send(It.IsAny<ObterAlunosBoletimResultadoProbabilidadePorTurmaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosBoletimResultadoProbabilidadeDto);

            var questoesProvaDto = ObterQuestoesProvaDto();
            mediator.Setup(m => m.Send(It.IsAny<ObterQuestoesBoletimResultaProbabilidadePorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoesProvaDto);

            var niveisProficiencia = ObterNiveisProficiencias();
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(niveisProficiencia);

            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(niveisProficiencia);

            mediator.Setup(m => m.Send(It.IsAny<CalcularProbabilidadeCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<double>(50.32));

            var resultado = await tratarTurmaBoletimResultadoProbabilidadeProvaUseCase.Executar(mensagemRabbit);
            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(questoesProvaDto.GroupBy(x => x.CodigoHabilidade).Count()));
        }

        [Fact]
        public async Task Nao_Deve_Publicar_Fila_Tratar_Boletim_Resultado_Probabilidade_Prova_Sem_Alunos()
        {
            var turmaBoletimResultadoProbabilidadeDto = ObterTurmaBoletimResultadoProbabilidadeDto();
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(turmaBoletimResultadoProbabilidadeDto), Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<ObterAlunosBoletimResultadoProbabilidadePorTurmaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<AlunoBoletimResultadoProbabilidadeDto>)null!);

            var questoesProvaDto = ObterQuestoesProvaDto();
            mediator.Setup(m => m.Send(It.IsAny<ObterQuestoesBoletimResultaProbabilidadePorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoesProvaDto);

            var niveisProficiencia = ObterNiveisProficiencias();
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(niveisProficiencia);

            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(niveisProficiencia);

            mediator.Setup(m => m.Send(It.IsAny<CalcularProbabilidadeCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<double>(50.32));

            var resultado = await tratarTurmaBoletimResultadoProbabilidadeProvaUseCase.Executar(mensagemRabbit);
            Assert.False(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Publicar_Fila_Tratar_Boletim_Resultado_Probabilidade_Prova_Sem_Questoes()
        {
            var turmaBoletimResultadoProbabilidadeDto = ObterTurmaBoletimResultadoProbabilidadeDto();
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(turmaBoletimResultadoProbabilidadeDto), Guid.NewGuid());

            var alunosBoletimResultadoProbabilidadeDto = ObterAlunosBoletimResultadoProbabilidadeDto();
            mediator.Setup(m => m.Send(It.IsAny<ObterAlunosBoletimResultadoProbabilidadePorTurmaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosBoletimResultadoProbabilidadeDto);

            mediator.Setup(m => m.Send(It.IsAny<ObterQuestoesBoletimResultaProbabilidadePorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<QuestaoProvaDto>)null!);

            var niveisProficiencia = ObterNiveisProficiencias();
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(niveisProficiencia);

            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(niveisProficiencia);

            mediator.Setup(m => m.Send(It.IsAny<CalcularProbabilidadeCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<double>(50.32));

            var resultado = await tratarTurmaBoletimResultadoProbabilidadeProvaUseCase.Executar(mensagemRabbit);
            Assert.False(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Ocorrer_Excecao()
        {
            var turmaBoletimResultadoProbabilidadeDto = ObterTurmaBoletimResultadoProbabilidadeDto();
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(turmaBoletimResultadoProbabilidadeDto), Guid.NewGuid());

            var alunosBoletimResultadoProbabilidadeDto = ObterAlunosBoletimResultadoProbabilidadeDto();
            mediator.Setup(m => m.Send(It.IsAny<ObterAlunosBoletimResultadoProbabilidadePorTurmaIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro"));

            var questoesProvaDto = ObterQuestoesProvaDto();
            mediator.Setup(m => m.Send(It.IsAny<ObterQuestoesBoletimResultaProbabilidadePorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro"));

            var niveisProficiencia = ObterNiveisProficiencias();
            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro"));

            mediator.Setup(m => m.Send(It.IsAny<ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro"));

            mediator.Setup(m => m.Send(It.IsAny<CalcularProbabilidadeCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro"));

            var resultado = await tratarTurmaBoletimResultadoProbabilidadeProvaUseCase.Executar(mensagemRabbit);
            Assert.False(resultado);
            serviceLog.Verify(x => x.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        private TurmaBoletimResultadoProbabilidadeDto ObterTurmaBoletimResultadoProbabilidadeDto()
        {
            return new TurmaBoletimResultadoProbabilidadeDto
            {
                AnoEscolar = 5,
                AnoLetivo = DateTime.Now.Year,
                DisciplinaId = 4,
                ProvaId = 1,
                TurmaId = 1,
                TurmaNome = "5A",
                UeId = 1
            };
        }

        private List<AlunoBoletimResultadoProbabilidadeDto> ObterAlunosBoletimResultadoProbabilidadeDto()
        {
            return new List<AlunoBoletimResultadoProbabilidadeDto>
            {
                new AlunoBoletimResultadoProbabilidadeDto
                {
                    Codigo = 123456,
                    UeId = 1,
                    TurmaNome = "5A",
                    NivelProficiencia = 1,
                    TurmaId = 1,
                    Nome = "Aluno Teste",
                    Proficiencia = 50.32,
                },
                new AlunoBoletimResultadoProbabilidadeDto
                {
                    Codigo = 7891011,
                    UeId = 1,
                    TurmaNome = "5A",
                    NivelProficiencia = 1,
                    TurmaId = 1,
                    Nome = "Aluno Teste",
                    Proficiencia = 50.32,
                },
            };
        }

        private List<QuestaoProvaDto> ObterQuestoesProvaDto()
        {
            return new List<QuestaoProvaDto>
            {
                new QuestaoProvaDto
                {
                    AcertoCasual = 0.5,
                    CodigoHabilidade = "H001",
                    DescricaoHabilidade = "Habilidade 1",
                    Dificuldade = 0.3,
                    Discriminacao = 0.2,
                    QuestaoLegadoId = 1001,
                    HabilidadeId = 1
                },
                new QuestaoProvaDto
                {
                    AcertoCasual = 0.6,
                    CodigoHabilidade = "H002",
                    DescricaoHabilidade = "Habilidade 2",
                    Dificuldade = 0.4,
                    Discriminacao = 0.3,
                    QuestaoLegadoId = 1002,
                    HabilidadeId = 2
                }
            };
        }

        private List<NivelProficiencia> ObterNiveisProficiencias()
        {
            return new List<NivelProficiencia>
            {
                new NivelProficiencia
                {
                    Codigo = (int)TipoNivelProficiencia.AbaixoBasico,
                    Ano = 5,
                    Descricao = "Abaixo Básico",
                    DisciplinaId = 4,
                    ValorReferencia = 100
                },
                new NivelProficiencia
                {
                    Codigo = (int)TipoNivelProficiencia.Basico,
                    Ano = 5,
                    Descricao = "Básico",
                    DisciplinaId = 4,
                    ValorReferencia = 150
                }
            };
        }
    }
}
