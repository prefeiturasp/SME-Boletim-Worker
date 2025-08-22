using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Infra.Testes.EnvironmentVariables
{
    public class RabbitOptionsTeste
    {
        [Fact]
        public void Deve_Retornar_Valor_Secao_Esperado()
        {
            var secao = RabbitOptions.Secao;

            Assert.Equal("Rabbit", secao);
        }

        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var options = new RabbitOptions
            {
                HostName = "localhost",
                UserName = "user",
                Password = "bitnami",
                VirtualHost = "/",
                LimiteDeMensagensPorExecucao = 50,
                ForcarRecriarFilas = true
            };

            Assert.Equal("localhost", options.HostName);
            Assert.Equal("user", options.UserName);
            Assert.Equal("bitnami", options.Password);
            Assert.Equal("/", options.VirtualHost);
            Assert.Equal((ushort)50, options.LimiteDeMensagensPorExecucao);
            Assert.True(options.ForcarRecriarFilas);
        }
    }
}
