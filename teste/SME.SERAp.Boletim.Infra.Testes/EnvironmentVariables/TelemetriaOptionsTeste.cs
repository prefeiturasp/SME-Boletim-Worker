using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Infra.Testes.EnvironmentVariables
{
    public class TelemetriaOptionsTeste
    {
        [Fact]
        public void Deve_Retornar_Valor_Secao_Esperado()
        {
            var secao = TelemetriaOptions.Secao;

            Assert.Equal("Telemetria", secao);
        }

        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var options = new TelemetriaOptions
            {
                ApplicationInsights = true,
                Apm = false
            };

            Assert.True(options.ApplicationInsights);
            Assert.False(options.Apm);
        }
    }
}
