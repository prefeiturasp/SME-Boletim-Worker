using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using SME.SERAp.Boletim.Infra.Services;

namespace SME.SERAp.Boletim.Infra.Testes.Services
{
    public class ServicoTelemetriaTeste
    {
        private readonly TelemetriaOptions telemetriaOptions;
        private readonly ServicoTelemetria servicoTelemetria;

        public ServicoTelemetriaTeste()
        {
            telemetriaOptions = new TelemetriaOptions { ApplicationInsights = true, Apm = false };
            servicoTelemetria = new ServicoTelemetria(telemetriaOptions);
        }

        [Fact]
        public void Deve_Lancar_ArgumentNullException_Quando_TelemetriaOptions_ForNulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ServicoTelemetria(null));
        }

        [Fact]
        public void Deve_Criar_Transacao_Correta_Quando_IniciarTransacao_Com_ApplicationInsights()
        {
            var transacao = servicoTelemetria.IniciarTransacao("rotaTeste");

            Assert.NotNull(transacao);
            Assert.Equal("rotaTeste", transacao.Nome);
            Assert.NotNull(transacao.Temporizador);
            Assert.NotEqual(default(DateTime), transacao.InicioOperacao);
            Assert.True(transacao.Sucesso);
        }

        [Fact]
        public void Deve_Finalizar_Transacao_Sem_LancarErro_Quando_Apm_Desativado()
        {
            var transacao = servicoTelemetria.IniciarTransacao("rotaTeste");

            servicoTelemetria.FinalizarTransacao(transacao);
        }

        [Fact]
        public void Deve_Registrar_Excecao_Sem_LancarErro_Quando_Apm_Desativado()
        {
            var transacao = servicoTelemetria.IniciarTransacao("rotaTeste");
            var ex = new Exception("Erro teste");

            servicoTelemetria.RegistrarExcecao(transacao, ex);
        }

        [Fact]
        public async Task Deve_Retornar_Resultado_Correto_Quando_RegistrarComRetornoAsync_Sem_Parametros()
        {
            var result = await servicoTelemetria.RegistrarComRetornoAsync<int>(
                async () => await Task.FromResult<object>(42),
                "acaoTeste",
                "telemetria",
                "valor");

            Assert.Equal(42, result);
        }

        [Fact]
        public async Task Deve_Retornar_Resultado_Correto_Quando_RegistrarComRetornoAsync_Com_Parametros()
        {
            var result = await servicoTelemetria.RegistrarComRetornoAsync<int>(
                async () => await Task.FromResult<object>(99),
                "acaoTeste",
                "telemetria",
                "valor",
                "parametros");

            Assert.Equal(99, result);
        }

        [Fact]
        public void Deve_Retornar_Resultado_Correto_Quando_RegistrarComRetorno_Sincrono()
        {
            var result = servicoTelemetria.RegistrarComRetorno<int>(
                () => 123,
                "acaoTeste",
                "telemetria",
                "valor");

            Assert.Equal(123, result);
        }

        [Fact]
        public void Deve_Executar_Acao_Correta_Quando_Registrar_Sincrono()
        {
            bool executou = false;

            servicoTelemetria.Registrar(() => executou = true, "acaoTeste", "telemetria", "valor");

            Assert.True(executou);
        }

        [Fact]
        public async Task Deve_Executar_Acao_Correta_Quando_RegistrarAsync_Assincrono()
        {
            bool executou = false;

            await servicoTelemetria.RegistrarAsync(async () =>
            {
                executou = true;
                await Task.CompletedTask;
            }, "acaoTeste", "telemetria", "valor");

            Assert.True(executou);
        }
    }
}
