using MediatR;
using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresDetalhesPorProvaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaPorId;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;
using System.Text.Json;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UsesCases
{
    public class BuscarBoletinsEscolaresProvaUseCaseTeste
    {
        private readonly BuscarBoletinsEscolaresProvaUseCase buscarBoletinsEscolaresProvaUseCase;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IChannel> channel;
        private readonly Mock<IServicoLog> serviceLog;

        public BuscarBoletinsEscolaresProvaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            channel = new Mock<IChannel>();
            serviceLog = new Mock<IServicoLog>();
            buscarBoletinsEscolaresProvaUseCase = new BuscarBoletinsEscolaresProvaUseCase(mediator.Object, channel.Object, serviceLog.Object);
        }

        [Fact]
        public async Task Deve_Buscar_Boletins_Escolares_Prova()
        {
            var provaId = 1;
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(provaId), Guid.NewGuid());
            var boletimEscolarDetalhesDtos = ObterBoletinsEscolaresDetalhes(provaId);

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Prova { Id = provaId });

            mediator.Setup(m => m.Send(It.IsAny<ObterBoletinsEscolaresDetalhesPorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(boletimEscolarDetalhesDtos);

            mediator.Setup(m => m.Send(It.IsAny<ExcluirBoletinsEscolaresPorProvaIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await buscarBoletinsEscolaresProvaUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(boletimEscolarDetalhesDtos.Count));
        }

        [Fact]
        public async Task Deve_Validar_Id_Prova()
        {
            var provaId = 0;
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(provaId), Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Prova { Id = 1 });

            mediator.Setup(m => m.Send(It.IsAny<ObterBoletinsEscolaresDetalhesPorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ObterBoletinsEscolaresDetalhes(provaId));

            mediator.Setup(m => m.Send(It.IsAny<ExcluirBoletinsEscolaresPorProvaIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await buscarBoletinsEscolaresProvaUseCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Encontrar_Prova()
        {
            var provaId = 1;
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(provaId), Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Prova>(null!));

            mediator.Setup(m => m.Send(It.IsAny<ObterBoletinsEscolaresDetalhesPorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ObterBoletinsEscolaresDetalhes(provaId));

            mediator.Setup(m => m.Send(It.IsAny<ExcluirBoletinsEscolaresPorProvaIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await buscarBoletinsEscolaresProvaUseCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Publicar_Tratar_Boletim_Escolar_Prova()
        {
            var provaId = 1;
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(provaId), Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Prova { Id = provaId });

            mediator.Setup(m => m.Send(It.IsAny<ObterBoletinsEscolaresDetalhesPorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<BoletimEscolarDetalhesDto>>(null!));

            mediator.Setup(m => m.Send(It.IsAny<ExcluirBoletinsEscolaresPorProvaIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await buscarBoletinsEscolaresProvaUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private static List<BoletimEscolarDetalhesDto> ObterBoletinsEscolaresDetalhes(int provaId)
        {
            return new List<BoletimEscolarDetalhesDto>
            {
                new BoletimEscolarDetalhesDto { ProvaId = provaId, UeId = 1 },
                new BoletimEscolarDetalhesDto { ProvaId = provaId, UeId = 1 },
            };
        }
    }
}
