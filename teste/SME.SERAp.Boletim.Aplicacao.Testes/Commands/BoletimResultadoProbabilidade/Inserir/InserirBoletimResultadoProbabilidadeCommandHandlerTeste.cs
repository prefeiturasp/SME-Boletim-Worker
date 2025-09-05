using Moq;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Commands
{
    public class InserirBoletimResultadoProbabilidadeCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Inserir_Boletim_Resultado_Probabilidade()
        {
            var boletim = new BoletimResultadoProbabilidade();
            var retornoEsperado = 99L;

            var repositorio = new Mock<IRepositorioBoletimResultadoProbabilidade>();
            repositorio
                .Setup(r => r.IncluirAsync(boletim))
                .ReturnsAsync(retornoEsperado);

            var handler = new InserirBoletimResultadoProbabilidadeCommandHandler(repositorio.Object);
            var command = new InserirBoletimResultadoProbabilidadeCommand(boletim);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(retornoEsperado, resultado);
            repositorio.Verify(r => r.IncluirAsync(boletim), Times.Once);
        }
    }
}
