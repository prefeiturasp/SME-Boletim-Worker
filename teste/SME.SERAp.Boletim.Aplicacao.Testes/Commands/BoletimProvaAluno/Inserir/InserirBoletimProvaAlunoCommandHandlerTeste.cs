using Moq;
using System.Threading;
using System.Threading.Tasks;
using SME.SERAp.Boletim.Aplicacao;
using SME.SERAp.Boletim.Aplicacao.Commands;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Commands
{
    public class InserirBoletimProvaAlunoCommandHandlerTeste
    {
        [Fact]
        public async Task Deve_Inserir_Boletim_Prova_Aluno()
        {
            var boletimProvaAluno = new BoletimProvaAluno();
            var retornoEsperado = 42L;

            var repositorio = new Mock<IRepositorioBoletimProvaAluno>();
            repositorio.Setup(r => r.IncluirAsync(boletimProvaAluno)).ReturnsAsync(retornoEsperado);

            var handler = new InserirBoletimProvaAlunoCommandHandler(repositorio.Object);
            var command = new InserirBoletimProvaAlunoCommand(boletimProvaAluno);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(retornoEsperado, resultado);
            repositorio.Verify(r => r.IncluirAsync(boletimProvaAluno), Times.Once);
        }
    }
}
