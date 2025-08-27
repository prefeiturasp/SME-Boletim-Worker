using MediatR;
using Moq;
using SME.SERAp.Boletim.Aplicacao.Commands.LoteProva.AlterarStatusConsolidacao;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Dominio.Enums;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UseCases
{
    public class ConsolidarBoletimEscolarLoteUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ConsolidarBoletimEscolarLoteUseCase useCase;

        public ConsolidarBoletimEscolarLoteUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ConsolidarBoletimEscolarLoteUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Executar_Com_Sucesso_Quando_Lote_Ainda_Nao_Foi_Consolidado()
        {
            var loteId = 10;

            mediator.Setup(m => m.Send(It.IsAny<AlterarLoteProvaStatusConsolidacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await useCase.Executar(loteId);

            mediator.Verify(m => m.Send(It.Is<AlterarLoteProvaStatusConsolidacaoCommand>(
                c => c.IdLoteProva == loteId && c.StatusConsolidacao == LoteStatusConsolidacao.Pendente), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Nao_Executar_Novamente_Quando_Lote_Ja_Esta_Na_Lista()
        {
            var loteId = 20;

            mediator.Setup(m => m.Send(It.IsAny<AlterarLoteProvaStatusConsolidacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await useCase.Executar(loteId);
            await useCase.Executar(loteId);

            mediator.Verify(m => m.Send(It.IsAny<AlterarLoteProvaStatusConsolidacaoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Disparar_Publicacao_Na_Fila_Apos_Timeout()
        {
            var loteId = 30;

            mediator.Setup(m => m.Send(It.IsAny<AlterarLoteProvaStatusConsolidacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var delaySegundosPublicar = 2;
            await useCase.Executar(loteId, delaySegundosPublicar);

            await Task.Delay(TimeSpan.FromSeconds(delaySegundosPublicar));

            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Deve_Propagar_Excecao_Quando_Mediator_Lancar()
        {
            var loteId = 40;

            mediator.Setup(m => m.Send(It.IsAny<AlterarLoteProvaStatusConsolidacaoCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro inesperado"));

            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(loteId));
        }
    }
}
