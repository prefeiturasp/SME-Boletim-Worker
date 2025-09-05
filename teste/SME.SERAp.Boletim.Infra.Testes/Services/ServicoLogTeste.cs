using Microsoft.Extensions.Logging;
using Moq;
using SME.SERAp.Boletim.Dominio.Enums;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;
using SME.SERAp.Boletim.Infra.Services;

namespace SME.SERAp.Boletim.Infra.Testes.Services
{
    public class ServicoLogTeste
    {
        private readonly Mock<IServicoTelemetria> servicoTelemetria;
        private readonly Mock<ILogger<ServicoLog>> logger;
        private readonly RabbitLogOptions rabbitOptions;
        private readonly ServicoLog servicoLog;

        public ServicoLogTeste()
        {
            servicoTelemetria = new Mock<IServicoTelemetria>();
            logger = new Mock<ILogger<ServicoLog>>();
            rabbitOptions = new RabbitLogOptions
            {
                HostName = "localhost",
                UserName = "user",
                Password = "bitnami",
                VirtualHost = "/"
            };

            servicoLog = new ServicoLog(servicoTelemetria.Object, rabbitOptions, logger.Object);
        }

        [Fact]
        public void Deve_Lancar_ArgumentNullException_Se_ServicoTelemetria_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ServicoLog(null, rabbitOptions, logger.Object));
        }

        [Fact]
        public void Deve_Lancar_ArgumentNullException_Se_RabbitOptions_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ServicoLog(servicoTelemetria.Object, null, logger.Object));
        }

        [Fact]
        public void Deve_Lancar_ArgumentNullException_Se_Logger_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ServicoLog(servicoTelemetria.Object, rabbitOptions, null));
        }

        [Fact]
        public void Deve_Chamar_ServicoTelemetria_Utilizando_Registrar_Exception()
        {
            var ex = new Exception("Erro teste");

            servicoLog.Registrar(ex);

            servicoTelemetria.Verify(s =>
                s.Registrar(It.IsAny<Action>(),
                            "RabbitMQ",
                            "Salvar Log Via Rabbit",
                            RotasRabbit.RotaLogs),
                Times.Once);
        }

        [Fact]
        public void Deve_Chamar_ServicoTelemetria_Utilizando_Registrar_LogNivel()
        {
            servicoLog.Registrar(LogNivel.Critico, "Erro", "Obs", "Stack");

            servicoTelemetria.Verify(s =>
                s.Registrar(It.IsAny<Action>(),
                            "RabbitMQ",
                            "Salvar Log Via Rabbit",
                            RotasRabbit.RotaLogs),
                Times.Once);
        }

        [Fact]
        public void Deve_Chamar_ServicoTelemetria_Utilizando_Registrar_StringEException()
        {
            var ex = new Exception("Erro teste");

            servicoLog.Registrar("Mensagem", ex);

            servicoTelemetria.Verify(s =>
                s.Registrar(It.IsAny<Action>(),
                            "RabbitMQ",
                            "Salvar Log Via Rabbit",
                            RotasRabbit.RotaLogs),
                Times.Once);
        }
    }
}
