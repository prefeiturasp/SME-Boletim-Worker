using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Dominio.Enums;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Entities
{
    public class LogMensagemTeste
    {
        [Fact]
        public void Construtor_DeveAtribuirPropriedadesCorretamente()
        {
            string mensagem = "Erro ao processar mensagem";
            LogNivel nivel = LogNivel.Critico;
            string observacao = "Exceção lançada no método X";
            string rastreamento = "Stack trace...";
            string excecaoInterna = "System.NullReferenceException";
            string projeto = "Serap-Prova-Worker";

            var antes = DateTime.Now;
            var log = new LogMensagem(mensagem, nivel, observacao, rastreamento, excecaoInterna, projeto);
            var depois = DateTime.Now;

            Assert.Equal(mensagem, log.Mensagem);
            Assert.Equal(nivel, log.Nivel);
            Assert.Equal(observacao, log.Observacao);
            Assert.Equal(rastreamento, log.Rastreamento);
            Assert.Equal(excecaoInterna, log.ExcecaoInterna);
            Assert.Equal(projeto, log.Projeto);
            Assert.True(log.DataHora >= antes && log.DataHora <= depois);
        }

        [Fact]
        public void Construtor_DeveAtribuirProjetoPadraoQuandoNaoInformado()
        {
            var log = new LogMensagem("Mensagem", LogNivel.Informacao, "Obs");

            Assert.Equal("Serap-Prova-Worker", log.Projeto);
        }
    }
}
