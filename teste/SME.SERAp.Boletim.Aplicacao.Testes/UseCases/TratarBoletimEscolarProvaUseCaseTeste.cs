using MediatR;
using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;
using System.Text.Json;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UseCases
{
    public class TratarBoletimEscolarProvaUseCaseTeste
    {
        private readonly TratarBoletimEscolarProvaUseCase tratarBoletimEscolarProvaUseCase;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IChannel> channel;
        private readonly Mock<IServicoLog> serviceLog;

        public TratarBoletimEscolarProvaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            channel = new Mock<IChannel>();
            serviceLog = new Mock<IServicoLog>();
            tratarBoletimEscolarProvaUseCase = new TratarBoletimEscolarProvaUseCase(mediator.Object, channel.Object, serviceLog.Object);
        }

        [Fact]
        public async Task Deve_Inserir_Boletim_Escolar()
        {
            var boletimEscolarDetalhesDto = ObterBoletimEscolarDetalhesDto();

            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(boletimEscolarDetalhesDto), Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<InserirBoletimEscolarCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await tratarBoletimEscolarProvaUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(x => x.Send(It.Is<InserirBoletimEscolarCommand>(cmd =>
                 cmd.BoletimEscolar.UeId == boletimEscolarDetalhesDto.UeId &&
                cmd.BoletimEscolar.ProvaId == boletimEscolarDetalhesDto.ProvaId &&
                cmd.BoletimEscolar.ComponenteCurricular == boletimEscolarDetalhesDto.ComponenteCurricular &&
                cmd.BoletimEscolar.DisciplinaId == boletimEscolarDetalhesDto.DisciplinaId &&
                cmd.BoletimEscolar.AbaixoBasico == boletimEscolarDetalhesDto.AbaixoBasico &&
                cmd.BoletimEscolar.AbaixoBasicoPorcentagem == boletimEscolarDetalhesDto.AbaixoBasicoPorcentagem &&
                cmd.BoletimEscolar.Basico == boletimEscolarDetalhesDto.Basico &&
                cmd.BoletimEscolar.BasicoPorcentagem == boletimEscolarDetalhesDto.BasicoPorcentagem &&
                cmd.BoletimEscolar.Adequado == boletimEscolarDetalhesDto.Adequado &&
                cmd.BoletimEscolar.AdequadoPorcentagem == boletimEscolarDetalhesDto.AdequadoPorcentagem &&
                cmd.BoletimEscolar.Avancado == boletimEscolarDetalhesDto.Avancado &&
                cmd.BoletimEscolar.AvancadoPorcentagem == boletimEscolarDetalhesDto.AvancadoPorcentagem &&
                cmd.BoletimEscolar.Total == boletimEscolarDetalhesDto.Total &&
                cmd.BoletimEscolar.MediaProficiencia == boletimEscolarDetalhesDto.MediaProficiencia &&
                cmd.BoletimEscolar.NivelUeCodigo == boletimEscolarDetalhesDto.NivelUeCodigo &&
                cmd.BoletimEscolar.NivelUeDescricao == boletimEscolarDetalhesDto.NivelUeDescricao
            ), default), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Inserir_Boletim_Escolar()
        {
            var mensagemRabbit = new MensagemRabbit(string.Empty, Guid.NewGuid());

            var resultado = await tratarBoletimEscolarProvaUseCase.Executar(mensagemRabbit);
            mediator.Setup(m => m.Send(It.IsAny<InserirBoletimEscolarCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<InserirBoletimEscolarCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Ocorrer_Excecao()
        {
            var boletimEscolarDetalhesDto = ObterBoletimEscolarDetalhesDto();

            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(boletimEscolarDetalhesDto), Guid.NewGuid());

            mediator.Setup(x => x.Send(It.IsAny<InserirBoletimEscolarCommand>(), default)).ThrowsAsync(new Exception("Erro"));

            var resultado = await tratarBoletimEscolarProvaUseCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            serviceLog.Verify(x => x.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        private BoletimEscolarDetalhesDto ObterBoletimEscolarDetalhesDto()
        {
            return new BoletimEscolarDetalhesDto
            {
                Disciplina = "Matemática",
                ProvaId = 3,
                UeId = 4,
                DisciplinaId = 5,
                AnoEscolar = 2025,
                AbaixoBasico = 0.1M,
                AbaixoBasicoPorcentagem = 0.1M,
                Basico = 0.2M,
                BasicoPorcentagem = 0.2M,
                Adequado = 0.3M,
                AdequadoPorcentagem = 0.3M,
                Avancado = 0.4M,
                AvancadoPorcentagem = 0.4M,
                Total = 100,
                MediaProficiencia = 0.75M,
                NivelUeCodigo = 1,
                NivelUeDescricao = "Nível 1"
            };
        }
    }
}
