using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Infra.Testes.EnvironmentVariables
{
    public class RabbitLogOptionsTeste
    {
        [Fact]
        public void Deve_Retornar_Valor_Secao_Esperado()
        {
            var secao = RabbitLogOptions.Secao;

            Assert.Equal("RabbitLog", secao);
        }

        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var options = new RabbitLogOptions
            {
                HostName = "localhost",
                UserName = "user",
                Password = "bitnami",
                VirtualHost = "/"
            };

            Assert.Equal("localhost", options.HostName);
            Assert.Equal("user", options.UserName);
            Assert.Equal("bitnami", options.Password);
            Assert.Equal("/", options.VirtualHost);
        }
    }
}
