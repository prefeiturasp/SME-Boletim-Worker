using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Infra.Testes.EnvironmentVariables
{
    public class ElasticOptionsTeste
    {
        [Fact]
        public void Deve_Retornar_Valor_Secao_Esperado()
        {
            var secao = ElasticOptions.Secao;

            Assert.Equal("Elastic", secao);
        }

        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var options = new ElasticOptions
            {
                Urls = "http://localhost:0000",
                DefaultIndex = "default",
                PrefixIndex = "prefix",
                CertificateFingerprint = "fingerprint",
                Username = "user",
                Password = "pass"
            };

            Assert.Equal("http://localhost:0000", options.Urls);
            Assert.Equal("default", options.DefaultIndex);
            Assert.Equal("prefix", options.PrefixIndex);
            Assert.Equal("fingerprint", options.CertificateFingerprint);
            Assert.Equal("user", options.Username);
            Assert.Equal("pass", options.Password);
        }
    }
}
