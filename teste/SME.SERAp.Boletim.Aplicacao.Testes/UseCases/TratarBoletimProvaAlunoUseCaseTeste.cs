using MediatR;
using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolar;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterQuantidadeMensagensPorNomeFila;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Dominio.Enums;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;
using System.Text.Json;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UseCases
{
    public class TratarBoletimProvaAlunoUseCaseTeste
    {
        private readonly TratarBoletimProvaAlunoUseCase tratarBoletimProvaAlunoUseCase;
        private readonly Mock<IConsolidarBoletimEscolarLoteUseCase> consolidarBoletimEscolarUseCase;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IChannel> channel;
        private readonly Mock<IServicoLog> serviceLog;

        public TratarBoletimProvaAlunoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            channel = new Mock<IChannel>();
            serviceLog = new Mock<IServicoLog>();
            consolidarBoletimEscolarUseCase = new Mock<IConsolidarBoletimEscolarLoteUseCase>();
            tratarBoletimProvaAlunoUseCase = new TratarBoletimProvaAlunoUseCase(mediator.Object, channel.Object, serviceLog.Object, consolidarBoletimEscolarUseCase.Object);
        }

        [Fact]
        public async Task Deve_Inserir_Boletim_Prova_Aluno_E_Deve_Excluir_Anteriores()
        {
            var alunoProvaProficienciaBoletimDto = ObterAlunoProvaProficienciaBoletimDto();
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(alunoProvaProficienciaBoletimDto), Guid.NewGuid());

            var boletinsProvasAlunosPorProvaIdAlunoRaAnoEscola = ObterBoletinsProvasAlunosPorProvaIdAlunoRaAnoEscola();
            mediator.Setup(m => m.Send(It.IsAny<ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(boletinsProvasAlunosPorProvaIdAlunoRaAnoEscola);

            mediator.Setup(m => m.Send(It.IsAny<InserirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(m => m.Send(It.IsAny<ExcluirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(m => m.Send(It.IsAny<ObterQuantidadeMensagensPorNomeFilaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await tratarBoletimProvaAlunoUseCase.Executar(mensagemRabbit);
            Assert.True(resultado);
            mediator.Verify(x => x.Send(It.Is<InserirBoletimProvaAlunoCommand>(cmd =>
                 cmd.BoletimProvaAluno.ProvaStatus == alunoProvaProficienciaBoletimDto.ProvaStatus &&
                 cmd.BoletimProvaAluno.Turma == alunoProvaProficienciaBoletimDto.Turma &&
                 cmd.BoletimProvaAluno.ProvaId == alunoProvaProficienciaBoletimDto.ProvaId &&
                 cmd.BoletimProvaAluno.ProvaDescricao == alunoProvaProficienciaBoletimDto.NomeProva &&
                 cmd.BoletimProvaAluno.AlunoNome == alunoProvaProficienciaBoletimDto.NomeAluno &&
                 cmd.BoletimProvaAluno.AlunoRa == alunoProvaProficienciaBoletimDto.CodigoAluno &&
                 cmd.BoletimProvaAluno.AnoEscolar == alunoProvaProficienciaBoletimDto.AnoEscolar &&
                 cmd.BoletimProvaAluno.CodigoUe == alunoProvaProficienciaBoletimDto.CodigoUe &&
                 cmd.BoletimProvaAluno.NomeUe == alunoProvaProficienciaBoletimDto.NomeUe &&
                 cmd.BoletimProvaAluno.DreId == alunoProvaProficienciaBoletimDto.CodigoDre &&
                 cmd.BoletimProvaAluno.Disciplina == alunoProvaProficienciaBoletimDto.NomeDisciplina &&
                 cmd.BoletimProvaAluno.DisciplinaId == alunoProvaProficienciaBoletimDto.DisciplinaId &&
                 cmd.BoletimProvaAluno.ErroMedida == alunoProvaProficienciaBoletimDto.ErroMedida &&
                 cmd.BoletimProvaAluno.NivelCodigo == alunoProvaProficienciaBoletimDto.NivelCodigo &&
                 cmd.BoletimProvaAluno.Proficiencia == alunoProvaProficienciaBoletimDto.Proficiencia
            ), default), Times.Once);

            mediator.Verify(m => m.Send(It.IsAny<ExcluirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(boletinsProvasAlunosPorProvaIdAlunoRaAnoEscola.Count));
            consolidarBoletimEscolarUseCase.Verify(c => c.Executar(It.IsAny<long>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Inserir_Boletim_Prova_Aluno_E_Nao_Deve_Excluir_Anteriores()
        {
            var alunoProvaProficienciaBoletimDto = ObterAlunoProvaProficienciaBoletimDto();
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(alunoProvaProficienciaBoletimDto), Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<BoletimProvaAluno>>(null!));

            mediator.Setup(m => m.Send(It.IsAny<InserirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(m => m.Send(It.IsAny<ExcluirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(m => m.Send(It.IsAny<ObterQuantidadeMensagensPorNomeFilaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await tratarBoletimProvaAlunoUseCase.Executar(mensagemRabbit);
            Assert.True(resultado);
            mediator.Verify(x => x.Send(It.Is<InserirBoletimProvaAlunoCommand>(cmd =>
                 cmd.BoletimProvaAluno.ProvaStatus == alunoProvaProficienciaBoletimDto.ProvaStatus &&
                 cmd.BoletimProvaAluno.Turma == alunoProvaProficienciaBoletimDto.Turma &&
                 cmd.BoletimProvaAluno.ProvaId == alunoProvaProficienciaBoletimDto.ProvaId &&
                 cmd.BoletimProvaAluno.ProvaDescricao == alunoProvaProficienciaBoletimDto.NomeProva &&
                 cmd.BoletimProvaAluno.AlunoNome == alunoProvaProficienciaBoletimDto.NomeAluno &&
                 cmd.BoletimProvaAluno.AlunoRa == alunoProvaProficienciaBoletimDto.CodigoAluno &&
                 cmd.BoletimProvaAluno.AnoEscolar == alunoProvaProficienciaBoletimDto.AnoEscolar &&
                 cmd.BoletimProvaAluno.CodigoUe == alunoProvaProficienciaBoletimDto.CodigoUe &&
                 cmd.BoletimProvaAluno.NomeUe == alunoProvaProficienciaBoletimDto.NomeUe &&
                 cmd.BoletimProvaAluno.DreId == alunoProvaProficienciaBoletimDto.CodigoDre &&
                 cmd.BoletimProvaAluno.Disciplina == alunoProvaProficienciaBoletimDto.NomeDisciplina &&
                 cmd.BoletimProvaAluno.DisciplinaId == alunoProvaProficienciaBoletimDto.DisciplinaId &&
                 cmd.BoletimProvaAluno.ErroMedida == alunoProvaProficienciaBoletimDto.ErroMedida &&
                 cmd.BoletimProvaAluno.NivelCodigo == alunoProvaProficienciaBoletimDto.NivelCodigo &&
                 cmd.BoletimProvaAluno.Proficiencia == alunoProvaProficienciaBoletimDto.Proficiencia
            ), default), Times.Once);

            mediator.Verify(m => m.Send(It.IsAny<ExcluirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            consolidarBoletimEscolarUseCase.Verify(c => c.Executar(It.IsAny<long>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Inserir_Boletim_Prova_Aluno()
        {
            var mensagemRabbit = new MensagemRabbit(string.Empty, Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<BoletimProvaAluno>>(null!));

            mediator.Setup(m => m.Send(It.IsAny<InserirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(m => m.Send(It.IsAny<ExcluirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(m => m.Send(It.IsAny<ObterQuantidadeMensagensPorNomeFilaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await tratarBoletimProvaAlunoUseCase.Executar(mensagemRabbit);
            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<InserirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            consolidarBoletimEscolarUseCase.Verify(c => c.Executar(It.IsAny<long>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Ocorrer_Excecao()
        {
            var alunoProvaProficienciaBoletimDto = ObterAlunoProvaProficienciaBoletimDto();
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(alunoProvaProficienciaBoletimDto), Guid.NewGuid());

            var boletinsProvasAlunosPorProvaIdAlunoRaAnoEscola = ObterBoletinsProvasAlunosPorProvaIdAlunoRaAnoEscola();
            mediator.Setup(m => m.Send(It.IsAny<ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(boletinsProvasAlunosPorProvaIdAlunoRaAnoEscola);

            mediator.Setup(m => m.Send(It.IsAny<InserirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro"));

            mediator.Setup(m => m.Send(It.IsAny<ExcluirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro"));

            mediator.Setup(m => m.Send(It.IsAny<ObterQuantidadeMensagensPorNomeFilaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await tratarBoletimProvaAlunoUseCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            serviceLog.Verify(x => x.Registrar(It.IsAny<Exception>()), Times.Once);
            consolidarBoletimEscolarUseCase.Verify(c => c.Executar(It.IsAny<long>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Inserir_Boletim_Prova_Aluno_E_Consolidar_Boletim_Escolar()
        {
            var alunoProvaProficienciaBoletimDto = ObterAlunoProvaProficienciaBoletimDto();
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(alunoProvaProficienciaBoletimDto), Guid.NewGuid());

            var boletinsProvasAlunosPorProvaIdAlunoRaAnoEscola = ObterBoletinsProvasAlunosPorProvaIdAlunoRaAnoEscola();
            mediator.Setup(m => m.Send(It.IsAny<ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(boletinsProvasAlunosPorProvaIdAlunoRaAnoEscola);

            mediator.Setup(m => m.Send(It.IsAny<InserirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(m => m.Send(It.IsAny<ExcluirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(m => m.Send(It.IsAny<ObterQuantidadeMensagensPorNomeFilaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            var resultado = await tratarBoletimProvaAlunoUseCase.Executar(mensagemRabbit);
            Assert.True(resultado);
            mediator.Verify(x => x.Send(It.Is<InserirBoletimProvaAlunoCommand>(cmd =>
                 cmd.BoletimProvaAluno.ProvaStatus == alunoProvaProficienciaBoletimDto.ProvaStatus &&
                 cmd.BoletimProvaAluno.Turma == alunoProvaProficienciaBoletimDto.Turma &&
                 cmd.BoletimProvaAluno.ProvaId == alunoProvaProficienciaBoletimDto.ProvaId &&
                 cmd.BoletimProvaAluno.ProvaDescricao == alunoProvaProficienciaBoletimDto.NomeProva &&
                 cmd.BoletimProvaAluno.AlunoNome == alunoProvaProficienciaBoletimDto.NomeAluno &&
                 cmd.BoletimProvaAluno.AlunoRa == alunoProvaProficienciaBoletimDto.CodigoAluno &&
                 cmd.BoletimProvaAluno.AnoEscolar == alunoProvaProficienciaBoletimDto.AnoEscolar &&
                 cmd.BoletimProvaAluno.CodigoUe == alunoProvaProficienciaBoletimDto.CodigoUe &&
                 cmd.BoletimProvaAluno.NomeUe == alunoProvaProficienciaBoletimDto.NomeUe &&
                 cmd.BoletimProvaAluno.DreId == alunoProvaProficienciaBoletimDto.CodigoDre &&
                 cmd.BoletimProvaAluno.Disciplina == alunoProvaProficienciaBoletimDto.NomeDisciplina &&
                 cmd.BoletimProvaAluno.DisciplinaId == alunoProvaProficienciaBoletimDto.DisciplinaId &&
                 cmd.BoletimProvaAluno.ErroMedida == alunoProvaProficienciaBoletimDto.ErroMedida &&
                 cmd.BoletimProvaAluno.NivelCodigo == alunoProvaProficienciaBoletimDto.NivelCodigo &&
                 cmd.BoletimProvaAluno.Proficiencia == alunoProvaProficienciaBoletimDto.Proficiencia
            ), default), Times.Once);

            mediator.Verify(m => m.Send(It.IsAny<ExcluirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(boletinsProvasAlunosPorProvaIdAlunoRaAnoEscola.Count));
            consolidarBoletimEscolarUseCase.Verify(c => c.Executar(It.IsAny<long>(), It.IsAny<int>()), Times.Once);
        }

        private List<BoletimProvaAluno> ObterBoletinsProvasAlunosPorProvaIdAlunoRaAnoEscola()
        {
            return new List<BoletimProvaAluno>
            {
                new BoletimProvaAluno(1, "1234", "EMEF", 1, "PSA", 5, "5A", 1234567, "Aluno", "Matemática", 4, ProvaStatus.Finalizado, 100.32M, 0.35M, 1),
                new BoletimProvaAluno(1, "1234", "EMEF", 1, "PSA", 5, "5A", 1234567, "Aluno", "Matemática", 4, ProvaStatus.Finalizado, 100.20M, 0.35M, 1)
            };
        }

        private AlunoProvaProficienciaBoletimDto ObterAlunoProvaProficienciaBoletimDto()
        {
            return new AlunoProvaProficienciaBoletimDto
            {
                AnoEscolar = 5,
                BoletimLoteId = 1,
                CodigoAluno = 1234567,
                CodigoDre = 1,
                CodigoUe = "1234",
                DisciplinaId = 4,
                ErroMedida = 0.35M,
                NivelCodigo = 1,
                NomeAluno = "Aluno",
                NomeDisciplina = "Matemática",
                NomeProva = "PSA",
                NomeUe = "EMEF",
                Proficiencia = 120.23M,
                ProvaId = 1,
                ProvaStatus = ProvaStatus.Finalizado,
                Turma = "5A"
            };
        }
    }
}
