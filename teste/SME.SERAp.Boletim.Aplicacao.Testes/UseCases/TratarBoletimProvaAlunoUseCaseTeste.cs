using MediatR;
using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicarFilaRabbitSerapEstudante;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolar;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaAnoOriginal;
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

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaAnoOriginalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string)null!);

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
            mediator.Verify(m => m.Send(It.Is<PublicarFilaRabbitSerapEstudanteCommand>(cmd => cmd.NomeFila == RotasRabbit.BuscarAlunoProvaSpProficiencia), It.IsAny<CancellationToken>()), Times.Once);
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

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaAnoOriginalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string)null!);

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
            mediator.Verify(m => m.Send(It.Is<PublicarFilaRabbitSerapEstudanteCommand>(cmd => cmd.NomeFila == RotasRabbit.BuscarAlunoProvaSpProficiencia), It.IsAny<CancellationToken>()), Times.Once);
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
            mediator.Verify(m => m.Send(It.Is<PublicarFilaRabbitSerapEstudanteCommand>(cmd => cmd.NomeFila == RotasRabbit.BuscarAlunoProvaSpProficiencia), It.IsAny<CancellationToken>()), Times.Never);
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

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaAnoOriginalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string)null!);

            var resultado = await tratarBoletimProvaAlunoUseCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            serviceLog.Verify(x => x.Registrar(It.IsAny<Exception>()), Times.Once);
            consolidarBoletimEscolarUseCase.Verify(c => c.Executar(It.IsAny<long>(), It.IsAny<int>()), Times.Never);
            mediator.Verify(m => m.Send(It.Is<PublicarFilaRabbitSerapEstudanteCommand>(cmd => cmd.NomeFila == RotasRabbit.BuscarAlunoProvaSpProficiencia), It.IsAny<CancellationToken>()), Times.Never);
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

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaAnoOriginalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string)null!);

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
            mediator.Verify(m => m.Send(It.Is<PublicarFilaRabbitSerapEstudanteCommand>(cmd => cmd.NomeFila == RotasRabbit.BuscarAlunoProvaSpProficiencia), It.IsAny<CancellationToken>()), Times.Once);
            consolidarBoletimEscolarUseCase.Verify(c => c.Executar(It.IsAny<long>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Atualizar_Dados_Com_Prova_Ano_Original_Diferente_E_Boletim_Encontrado()
        {
            var alunoProvaProficienciaBoletimDto = ObterAlunoProvaProficienciaBoletimDto();
            alunoProvaProficienciaBoletimDto.AnoEscolar = 5;
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

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaAnoOriginalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("6");

            mediator.Setup(m => m.Send(It.IsAny<ObterAnoProvaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2024);

            var boletimUltimaTurma = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 999L,
                CodigoUe = "UE999",
                NomeUe = "Escola Updated",
                AnoEscolar = 6,
                Turma = "6B",
                NivelCodigo = 4
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(boletimUltimaTurma);

            var resultado = await tratarBoletimProvaAlunoUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            mediator.Verify(x => x.Send(It.Is<InserirBoletimProvaAlunoCommand>(cmd =>
                 cmd.BoletimProvaAluno.DreId == boletimUltimaTurma.CodigoDre &&
                 cmd.BoletimProvaAluno.CodigoUe == boletimUltimaTurma.CodigoUe &&
                 cmd.BoletimProvaAluno.NomeUe == boletimUltimaTurma.NomeUe &&
                 cmd.BoletimProvaAluno.AnoEscolar == boletimUltimaTurma.AnoEscolar &&
                 cmd.BoletimProvaAluno.Turma == boletimUltimaTurma.Turma &&
                 cmd.BoletimProvaAluno.NivelCodigo == boletimUltimaTurma.NivelCodigo
            ), default), Times.Once);

            mediator.Verify(m => m.Send(It.IsAny<ObterProvaAnoOriginalQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Usar_Ano_Atual_Quando_Ano_Prova_Nao_Encontrado()
        {
            var alunoProvaProficienciaBoletimDto = ObterAlunoProvaProficienciaBoletimDto();
            alunoProvaProficienciaBoletimDto.AnoEscolar = 5;
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

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaAnoOriginalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("6");

            mediator.Setup(m => m.Send(It.IsAny<ObterAnoProvaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int?)null);

            var boletimUltimaTurma = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 999L,
                CodigoUe = "UE999",
                NomeUe = "Escola",
                AnoEscolar = 6,
                Turma = "6B",
                NivelCodigo = 4
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(boletimUltimaTurma);

            var resultado = await tratarBoletimProvaAlunoUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            mediator.Verify(m => m.Send(It.IsAny<ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Atualizar_Dados_Quando_Prova_Ano_Original_Igual_Ano_Escolar()
        {
            var alunoProvaProficienciaBoletimDto = ObterAlunoProvaProficienciaBoletimDto();
            alunoProvaProficienciaBoletimDto.AnoEscolar = 5;
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(alunoProvaProficienciaBoletimDto), Guid.NewGuid());

            var boletinsProvasAlunosPorProvaIdAlunoRaAnoEscola = new List<BoletimProvaAluno>();
            mediator.Setup(m => m.Send(It.IsAny<ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(boletinsProvasAlunosPorProvaIdAlunoRaAnoEscola);

            mediator.Setup(m => m.Send(It.IsAny<InserirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(m => m.Send(It.IsAny<ObterQuantidadeMensagensPorNomeFilaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaAnoOriginalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("5");

            var resultado = await tratarBoletimProvaAlunoUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            mediator.Verify(m => m.Send(It.IsAny<ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Atualizar_Quando_Boletim_Ultima_Turma_Nulo()
        {
            var alunoProvaProficienciaBoletimDto = ObterAlunoProvaProficienciaBoletimDto();
            alunoProvaProficienciaBoletimDto.AnoEscolar = 5;
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

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaAnoOriginalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("6");

            mediator.Setup(m => m.Send(It.IsAny<ObterAnoProvaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2024);

            mediator.Setup(m => m.Send(It.IsAny<ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((BoletimProvaAlunoUltimaTurmaAlunoDto)null!);

            var resultado = await tratarBoletimProvaAlunoUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            mediator.Verify(m => m.Send(It.IsAny<ExcluirBoletimProvaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(boletinsProvasAlunosPorProvaIdAlunoRaAnoEscola.Count));
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
