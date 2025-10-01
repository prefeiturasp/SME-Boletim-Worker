using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Infra.Testes.EnvironmentVariables
{
    public class ConnectionStringOptionsTeste
    {
        [Fact]
        public void Deve_Retornar_Valor_Secao_Esperado()
        {
            var secao = ConnectionStringOptions.Secao;

            Assert.Equal("ConnectionStrings", secao);
        }

        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var options = new ConnectionStringOptions
            {
                ApiSerapExterna = "valor-api-serap-externa",
                ApiSerap = "valor-api-serap",
                ApiSerapLeitura = "valor-api-serap-leitura",
                ApiSgp = "valor-api-sgp",
                Eol = "valor-eol",
                CoreSSO = "valor-core-sso",
                ProvaSP = "valor-prova-sp"
            };

            Assert.Equal("valor-api-serap-externa", options.ApiSerapExterna);
            Assert.Equal("valor-api-serap", options.ApiSerap);
            Assert.Equal("valor-api-serap-leitura", options.ApiSerapLeitura);
            Assert.Equal("valor-api-sgp", options.ApiSgp);
            Assert.Equal("valor-eol", options.Eol);
            Assert.Equal("valor-core-sso", options.CoreSSO);
            Assert.Equal("valor-prova-sp", options.ProvaSP);
        }
    }
}
