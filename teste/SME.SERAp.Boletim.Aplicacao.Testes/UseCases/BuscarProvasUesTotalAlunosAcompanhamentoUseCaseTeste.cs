using MediatR;
using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Exceptions;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UseCases
{
    public class BuscarProvasUesTotalAlunosAcompanhamentoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IChannel> channel;
        private readonly Mock<IServicoLog> servicoLog;
        private readonly BuscarProvasUesTotalAlunosAcompanhamentoUseCase useCase;

        public BuscarProvasUesTotalAlunosAcompanhamentoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            channel = new Mock<IChannel>();
            servicoLog = new Mock<IServicoLog>();
            useCase = new BuscarProvasUesTotalAlunosAcompanhamentoUseCase(
                mediator.Object,
                channel.Object,
                servicoLog.Object
            );
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Mensagem_Nao_Eh_Numero()
        {
            var mensagem = new MensagemRabbit("invalido", Guid.NewGuid());

            var resultado = await useCase.Executar(mensagem);

            Assert.False(resultado);
            servicoLog.Verify(x => x.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_LoteId_Igual_Zero()
        {
            var mensagem = new MensagemRabbit("0", Guid.NewGuid());

            var resultado = await useCase.Executar(mensagem);

            Assert.False(resultado);
            servicoLog.Verify(x => x.Registrar(It.IsAny<NegocioException>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_True_Quando_Nao_Existir_Provas_No_Lote()
        {
            var mensagem = new MensagemRabbit("123", Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<ObterProvasTaiPorLoteIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProvaDto>());

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Deve_Retornar_True_Quando_Nao_Existir_Ues()
        {
            var mensagem = new MensagemRabbit("123", Guid.NewGuid());

            var provas = new List<ProvaDto>
            {
                new ProvaDto { Id = 1, LoteId = 123, AnoEscolar = 5, Inicio = new DateTime(2025, 1, 1) }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterProvasTaiPorLoteIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(provas);

            mediator.Setup(m => m.Send(It.IsAny<ObterUesPorAnosEscolaresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<UeDto>());

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Deve_Publicar_Mensagens_Quando_Existirem_Provas_E_Ues()
        {
            var mensagem = new MensagemRabbit("123", Guid.NewGuid());

            var provas = new List<ProvaDto>
            {
                new ProvaDto { Id = 1, LoteId = 123, AnoEscolar = 5, Inicio = new DateTime(2025, 1, 1) }
            };

            var ues = new List<UeDto>
            {
                new UeDto { Id = 10, DreId = 20 }
            };

            mediator.Setup(m => m.Send(It.IsAny<ObterProvasTaiPorLoteIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(provas);

            mediator.Setup(m => m.Send(It.IsAny<ObterUesPorAnosEscolaresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ues);

            mediator.Setup(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Mediator_Lancar_Excecao()
        {
            var mensagem = new MensagemRabbit("123", Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<ObterProvasTaiPorLoteIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro inesperado"));

            var resultado = await useCase.Executar(mensagem);

            Assert.False(resultado);
            servicoLog.Verify(x => x.Registrar(It.IsAny<Exception>()), Times.Once);
        }
    }
}
