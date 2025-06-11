using MediatR;
using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;
using System.Text.Json;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UsesCases
{
    public class TrataBoletimResultadoProbabilidadeProvaUseCaseTeste
    {
        private readonly TrataBoletimResultadoProbabilidadeProvaUseCase trataBoletimResultadoProbabilidadeProvaUseCase;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IChannel> channel;
        private readonly Mock<IServicoLog> serviceLog;

        public TrataBoletimResultadoProbabilidadeProvaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            channel = new Mock<IChannel>();
            serviceLog = new Mock<IServicoLog>();
            trataBoletimResultadoProbabilidadeProvaUseCase = new TrataBoletimResultadoProbabilidadeProvaUseCase(mediator.Object, channel.Object, serviceLog.Object);
        }

        [Fact]
        public async Task Deve_Inserir_Boletim_Resultado_Probabilidade()
        {
            var boletimResultadoProbabilidadeDto = new BoletimResultadoProbabilidadeDto
            {
                HabilidadeId = 1,
                CodigoHabilidade = "HAB001",
                HabilidadeDescricao = "Descrição",
                TurmaDescricao = "Turma A",
                TurmaId = 2,
                ProvaId = 3,
                UeId = 4,
                DisciplinaId = 5,
                AnoEscolar = 2025,
                AbaixoDoBasico = 0.1,
                Basico = 0.2,
                Adequado = 0.3,
                Avancado = 0.4
            };

            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(boletimResultadoProbabilidadeDto), Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<InserirBoletimResultadoProbabilidadeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await trataBoletimResultadoProbabilidadeProvaUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(x => x.Send(It.Is<InserirBoletimResultadoProbabilidadeCommand>(cmd =>
                cmd.BoletimResultadoProbabilidade.HabilidadeId == boletimResultadoProbabilidadeDto.HabilidadeId &&
                cmd.BoletimResultadoProbabilidade.CodigoHabilidade == boletimResultadoProbabilidadeDto.CodigoHabilidade &&
                cmd.BoletimResultadoProbabilidade.HabilidadeDescricao == boletimResultadoProbabilidadeDto.HabilidadeDescricao &&
                cmd.BoletimResultadoProbabilidade.TurmaDescricao == boletimResultadoProbabilidadeDto.TurmaDescricao &&
                cmd.BoletimResultadoProbabilidade.TurmaId == boletimResultadoProbabilidadeDto.TurmaId &&
                cmd.BoletimResultadoProbabilidade.ProvaId == boletimResultadoProbabilidadeDto.ProvaId &&
                cmd.BoletimResultadoProbabilidade.UeId == boletimResultadoProbabilidadeDto.UeId &&
                cmd.BoletimResultadoProbabilidade.DisciplinaId == boletimResultadoProbabilidadeDto.DisciplinaId &&
                cmd.BoletimResultadoProbabilidade.AnoEscolar == boletimResultadoProbabilidadeDto.AnoEscolar &&
                cmd.BoletimResultadoProbabilidade.AbaixoDoBasico == boletimResultadoProbabilidadeDto.AbaixoDoBasico &&
                cmd.BoletimResultadoProbabilidade.Basico == boletimResultadoProbabilidadeDto.Basico &&
                cmd.BoletimResultadoProbabilidade.Adequado == boletimResultadoProbabilidadeDto.Adequado &&
                cmd.BoletimResultadoProbabilidade.Avancado == boletimResultadoProbabilidadeDto.Avancado
            ), default), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Inserir_Boletim_Resultado_Probabilidade()
        {
            var mensagemRabbit = new MensagemRabbit(string.Empty, Guid.NewGuid());

            var resultado = await trataBoletimResultadoProbabilidadeProvaUseCase.Executar(mensagemRabbit);
            mediator.Setup(m => m.Send(It.IsAny<InserirBoletimResultadoProbabilidadeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<InserirBoletimResultadoProbabilidadeCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Ocorrer_Excecao()
        {
            var boletimResultadoProbabilidadeDto = new BoletimResultadoProbabilidadeDto
            {
                HabilidadeId = 1,
                CodigoHabilidade = "HAB001",
                HabilidadeDescricao = "Descrição",
                TurmaDescricao = "Turma A",
                TurmaId = 2,
                ProvaId = 3,
                UeId = 4,
                DisciplinaId = 5,
                AnoEscolar = 2025,
                AbaixoDoBasico = 0.1,
                Basico = 0.2,
                Adequado = 0.3,
                Avancado = 0.4
            };

            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(boletimResultadoProbabilidadeDto), Guid.NewGuid());

            mediator.Setup(x => x.Send(It.IsAny<InserirBoletimResultadoProbabilidadeCommand>(), default)).ThrowsAsync(new Exception("Erro"));

            var resultado = await trataBoletimResultadoProbabilidadeProvaUseCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            serviceLog.Verify(x => x.Registrar(It.IsAny<Exception>()), Times.Once);
        }
    }
}
