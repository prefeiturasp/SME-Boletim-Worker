using Microsoft.Extensions.Logging;
using Moq;
using Polly;
using Polly.Registry;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;
using SME.SERAp.Boletim.Infra.Policies;
using SME.SERAp.Boletim.Infra.Services;

namespace SME.SERAp.Boletim.Infra.Testes.Services
{
    public class ServicoMensageriaTeste
    {
        private readonly Mock<IServicoTelemetria> servicoTelemetria;
        private readonly Mock<IReadOnlyPolicyRegistry<string>> registry;
        private readonly Mock<IAsyncPolicy> policy;
        private readonly Mock<ILogger<ServicoLog>> logger;
        private readonly RabbitOptions rabbitOptions;
        private readonly ServicoMensageria servicoMensageria;

        public ServicoMensageriaTeste()
        {
            servicoTelemetria = new Mock<IServicoTelemetria>();
            registry = new Mock<IReadOnlyPolicyRegistry<string>>();
            policy = new Mock<IAsyncPolicy>();
            logger = new Mock<ILogger<ServicoLog>>();

            registry.Setup(r => r.Get<IAsyncPolicy>(PoliticaPolly.PublicaFila))
                         .Returns(policy.Object);

            rabbitOptions = new RabbitOptions
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/"
            };

            servicoMensageria = new ServicoMensageria(rabbitOptions, servicoTelemetria.Object, registry.Object, logger.Object);
        }

        [Fact]
        public void Deve_Lancar_ArgumentNullException_Se_RabbitOptions_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ServicoMensageria(null, servicoTelemetria.Object, registry.Object, logger.Object));
        }

        [Fact]
        public void Deve_Lancar_ArgumentNullException_Se_ServicoTelemetria_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ServicoMensageria(rabbitOptions, null, registry.Object, logger.Object));
        }

        [Fact]
        public void Deve_Lancar_ArgumentNullException_Se_Logger_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ServicoMensageria(rabbitOptions, servicoTelemetria.Object, registry.Object, null));
        }

        [Fact]
        public async Task Deve_Chamar_ServicoTelemetria_RegistrarAsync_Utilizando_Publicar()
        {
            var mensagem = new MensagemRabbit(new { Texto = "Teste" }, Guid.NewGuid());
            policy.Setup(p => p.ExecuteAsync(It.IsAny<Func<Task>>()))
                       .Returns<Func<Task>>(f => f());

            var result = await servicoMensageria.Publicar(mensagem, "rotaTeste", "exchangeTeste", "acaoTeste");

            Assert.True(result);

            servicoTelemetria.Verify(s =>
                s.RegistrarAsync(It.IsAny<Func<Task>>(), "acaoTeste", "rotaTeste", string.Empty),
                Times.Once);
        }
    }
}
