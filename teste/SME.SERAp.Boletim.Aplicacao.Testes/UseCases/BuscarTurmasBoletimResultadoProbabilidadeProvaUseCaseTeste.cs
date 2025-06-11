using MediatR;
using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.BoletimResultadoProbabilidade.ExcluirPorProvaId;
using SME.SERAp.Boletim.Aplicacao.Commands.LoteProva.AlterarStatusConsolidacao;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimLoteProvaPendentes;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaPorId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterTurmasBoletimResultadoProbabilidadePorProvaId;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;
using System.Text.Json;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UsesCases
{
    public class BuscarTurmasBoletimResultadoProbabilidadeProvaUseCaseTeste
    {
        private readonly BuscarTurmasBoletimResultadoProbabilidadeProvaUseCase buscarTurmasBoletimResultadoProbabilidadeProvaUseCase;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IChannel> channel;
        private readonly Mock<IServicoLog> serviceLog;

        public BuscarTurmasBoletimResultadoProbabilidadeProvaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            channel = new Mock<IChannel>();
            serviceLog = new Mock<IServicoLog>();
            buscarTurmasBoletimResultadoProbabilidadeProvaUseCase = new BuscarTurmasBoletimResultadoProbabilidadeProvaUseCase(mediator.Object, channel.Object, serviceLog.Object);
        }

        [Fact]
        public async Task Deve_Buscar_Turmas_Boletim_Resultado_Probabilidade_Prova()
        {
            var provaId = 1;
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(provaId), Guid.NewGuid());
            var turmasBoletinsResultadosProbabilidadesDtos = ObterTurmasBoletinsResultadosProbabilidadesDtos(provaId);
            mediator.Setup(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Prova { Id = provaId });

            mediator.Setup(m => m.Send(It.IsAny<ObterTurmasBoletimResultadoProbabilidadePorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasBoletinsResultadosProbabilidadesDtos);

            mediator.Setup(m => m.Send(It.IsAny<ExcluirBoletinsResultadosProbabilidadesPorProvaIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await buscarTurmasBoletimResultadoProbabilidadeProvaUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(turmasBoletinsResultadosProbabilidadesDtos.Count));
        }

        [Fact]
        public async Task Deve_Validar_Id_Prova()
        {
            var provaId = 0;
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(provaId), Guid.NewGuid());
            var turmasBoletinsResultadosProbabilidadesDtos = ObterTurmasBoletinsResultadosProbabilidadesDtos(provaId);
            mediator.Setup(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Prova { Id = 1 });

            mediator.Setup(m => m.Send(It.IsAny<ObterTurmasBoletimResultadoProbabilidadePorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasBoletinsResultadosProbabilidadesDtos);

            mediator.Setup(m => m.Send(It.IsAny<ExcluirBoletinsResultadosProbabilidadesPorProvaIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await buscarTurmasBoletimResultadoProbabilidadeProvaUseCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Encontrar_Prova()
        {
            var provaId = 1;
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(provaId), Guid.NewGuid());
            var turmasBoletinsResultadosProbabilidadesDtos = ObterTurmasBoletinsResultadosProbabilidadesDtos(provaId);
            mediator.Setup(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Prova>(null!));

            mediator.Setup(m => m.Send(It.IsAny<ObterTurmasBoletimResultadoProbabilidadePorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasBoletinsResultadosProbabilidadesDtos);

            mediator.Setup(m => m.Send(It.IsAny<ExcluirBoletinsResultadosProbabilidadesPorProvaIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await buscarTurmasBoletimResultadoProbabilidadeProvaUseCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Publicar_Fila_Tratar_Turma_Boletim_Resultado_Probabilidade_Prova()
        {
            var provaId = 1;
            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(provaId), Guid.NewGuid());
            mediator.Setup(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Prova { Id = provaId });

            mediator.Setup(m => m.Send(It.IsAny<ObterTurmasBoletimResultadoProbabilidadePorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<TurmaBoletimResultadoProbabilidadeDto>>(null!));

            mediator.Setup(m => m.Send(It.IsAny<ExcluirBoletinsResultadosProbabilidadesPorProvaIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await buscarTurmasBoletimResultadoProbabilidadeProvaUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private static List<TurmaBoletimResultadoProbabilidadeDto> ObterTurmasBoletinsResultadosProbabilidadesDtos(int provaId)
        {
            return new List<TurmaBoletimResultadoProbabilidadeDto>
            {
                new TurmaBoletimResultadoProbabilidadeDto { ProvaId = provaId, TurmaId = 1 },
                new TurmaBoletimResultadoProbabilidadeDto { ProvaId = provaId, TurmaId = 2 },
                new TurmaBoletimResultadoProbabilidadeDto { ProvaId = provaId, TurmaId = 3 },
            };
        }
    }
}
