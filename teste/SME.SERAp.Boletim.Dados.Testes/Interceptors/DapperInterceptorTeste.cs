using Dapper;
using Moq;
using SME.SERAp.Boletim.Dados.Interceptors;
using SME.SERAp.Boletim.Infra.Interfaces;
using System.Data;
using static Dapper.SqlMapper;

namespace SME.SERAp.Boletim.Dados.Testes.Interceptors
{
    [Collection("ColecaoMapeamentos")]
    public class DapperInterceptorTeste
    {
        private readonly Mock<IServicoTelemetria> servicoTelemetria;
        private readonly Mock<IDbConnection> connection;

        public DapperInterceptorTeste()
        {
            servicoTelemetria = new Mock<IServicoTelemetria>();
            connection = new Mock<IDbConnection>();
            DapperExtensionMethods.Init(servicoTelemetria.Object);
        }

        [Fact]
        public void Query_Deve_Chamar_Registrar_Com_Retorno()
        {
            var experado = new List<string> { "A" };
            servicoTelemetria
                .Setup(t => t.RegistrarComRetorno<string>(It.IsAny<Func<object>>(), "Postgres", "Query Test", "SQL"))
                .Returns(experado);

            var resultado = connection.Object.Query<string>("SQL", queryName: "Test");

            Assert.Equal(experado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetorno<string>(It.IsAny<Func<object>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }

        [Fact]
        public async Task Query_Async_Deve_Chamar_Registrar_Com_Retorno_Async()
        {
            var experado = new List<string> { "A" };
            servicoTelemetria
                .Setup(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"))
                .ReturnsAsync(experado);

            var resultado = await connection.Object.QueryAsync<string>("SQL", queryName: "Test");

            Assert.Equal(experado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }

        [Fact]
        public async Task QueryFirstOrDefaultAsync_Deve_Chamar_Registrar_Com_Retorno_Async()
        {
            var experado = "A";
            servicoTelemetria
                .Setup(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"))
                .ReturnsAsync(experado);

            var resultado = await connection.Object.QueryFirstOrDefaultAsync<string>("SQL", queryName: "Test");

            Assert.Equal(experado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }

        [Fact]
        public void Query_2Tipos_Deve_Chamar_Registrar_Com_Retorno()
        {
            var experado = new List<string> { "A" };
            servicoTelemetria
                .Setup(t => t.RegistrarComRetorno<string>(It.IsAny<Func<object>>(), "Postgres", "Query Test", "SQL"))
                .Returns(experado);

            var resultado = connection.Object.Query<string, string, string>("SQL", (f, s) => f + s, queryName: "Test");

            Assert.Equal(experado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetorno<string>(It.IsAny<Func<object>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }

        [Fact]
        public async Task QueryAsync_2Tipos_Deve_Chamar_Registrar_Com_Retorno_Async()
        {
            var experado = new List<string> { "A" };
            servicoTelemetria
                .Setup(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"))
                .ReturnsAsync(experado);

            var resultado = await connection.Object.QueryAsync<string, string, string>("SQL", (f, s) => f + s, queryName: "Test");

            Assert.Equal(experado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }

        [Fact]
        public void Execute_Deve_Chamar_Registrar_Com_Retorno()
        {
            int experado = 1;
            servicoTelemetria
                .Setup(t => t.RegistrarComRetorno<int>(It.IsAny<Func<object>>(), "Postgres", "Command Test", "SQL"))
                .Returns(experado);

            var resultado = connection.Object.Execute("SQL", queryName: "Test");

            Assert.Equal(experado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetorno<int>(It.IsAny<Func<object>>(), "Postgres", "Command Test", "SQL"), Times.Once);
        }

        [Fact]
        public async Task Execute_Async_Deve_Chamar_Registrar_Com_Retorno_Async()
        {
            int experado = 1;
            servicoTelemetria
                .Setup(t => t.RegistrarComRetornoAsync<int>(It.IsAny<Func<Task<object>>>(), "Postgres", "Command Test", "SQL"))
                .ReturnsAsync(experado);

            var resultado = await connection.Object.ExecuteAsync("SQL", queryName: "Test");

            Assert.Equal(experado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetornoAsync<int>(It.IsAny<Func<Task<object>>>(), "Postgres", "Command Test", "SQL"), Times.Once);
        }

        [Fact]
        public void Execute_CommandDefinition_Deve_Chamar_Registrar_Com_Retorno()
        {
            int experado = 1;
            var command = new CommandDefinition("SQL");
            servicoTelemetria
                .Setup(t => t.RegistrarComRetorno<int>(It.IsAny<Func<object>>(), "Postgres", "Command Test", command.ToString()))
                .Returns(experado);

            var resultado = connection.Object.Execute(command, queryName: "Test");

            Assert.Equal(experado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetorno<int>(It.IsAny<Func<object>>(), "Postgres", "Command Test", command.ToString()), Times.Once);
        }

        [Fact]
        public async Task QueryMultipleAsync_Deve_Chamar_Registrar_Com_Retorno_Async()
        {
            var experado = It.IsAny<GridReader>();
            servicoTelemetria
                .Setup(t => t.RegistrarComRetornoAsync<GridReader>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"))
                .ReturnsAsync(experado);

            var resultado = await connection.Object.QueryMultipleAsync("SQL", queryName: "Test");

            Assert.Equal(experado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetornoAsync<GridReader>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }

        [Fact]
        public void Query_3Tipos_Deve_Chamar_Registrar_Com_Retorno()
        {
            var esperado = new List<string> { "A" };
            servicoTelemetria
                .Setup(t => t.RegistrarComRetorno<string>(It.IsAny<Func<object>>(), "Postgres", "Query Test", "SQL"))
                .Returns(esperado);

            var resultado = connection.Object.Query<string, string, string, string>("SQL", (f, s, t) => f + s + t, queryName: "Test");

            Assert.Equal(esperado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetorno<string>(It.IsAny<Func<object>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }

        [Fact]
        public async Task QueryAsync_3Tipos_Deve_Chamar_Registrar_Com_Retorno_Async()
        {
            var esperado = new List<string> { "A" };
            servicoTelemetria
                .Setup(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"))
                .ReturnsAsync(esperado);

            var resultado = await connection.Object.QueryAsync<string, string, string, string>("SQL", (f, s, t) => f + s + t, queryName: "Test");

            Assert.Equal(esperado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }

        [Fact]
        public void Query_4Tipos_Deve_Chamar_Registrar_Com_Retorno()
        {
            var esperado = new List<string> { "A" };
            servicoTelemetria
                .Setup(t => t.RegistrarComRetorno<string>(It.IsAny<Func<object>>(), "Postgres", "Query Test", "SQL"))
                .Returns(esperado);

            var resultado = connection.Object.Query<string, string, string, string, string>("SQL", (a, b, c, d) => a + b + c + d, queryName: "Test");

            Assert.Equal(esperado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetorno<string>(It.IsAny<Func<object>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }

        [Fact]
        public async Task QueryAsync_4Tipos_Deve_Chamar_Registrar_Com_Retorno_Async()
        {
            var esperado = new List<string> { "A" };
            servicoTelemetria
                .Setup(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"))
                .ReturnsAsync(esperado);

            var resultado = await connection.Object.QueryAsync<string, string, string, string, string>("SQL", (a, b, c, d) => a + b + c + d, queryName: "Test");

            Assert.Equal(esperado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }

        [Fact]
        public void Query_5Tipos_Deve_Chamar_Registrar_Com_Retorno()
        {
            var esperado = new List<string> { "A" };
            servicoTelemetria
                .Setup(t => t.RegistrarComRetorno<string>(It.IsAny<Func<object>>(), "Postgres", "Query Test", "SQL"))
                .Returns(esperado);

            var resultado = connection.Object.Query<string, string, string, string, string, string>("SQL", (a, b, c, d, e) => a + b + c + d + e, queryName: "Test");

            Assert.Equal(esperado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetorno<string>(It.IsAny<Func<object>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }

        [Fact]
        public async Task QueryAsync_5Tipos_Deve_Chamar_Registrar_Com_Retorno_Async()
        {
            var esperado = new List<string> { "A" };
            servicoTelemetria
                .Setup(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"))
                .ReturnsAsync(esperado);

            var resultado = await connection.Object.QueryAsync<string, string, string, string, string, string>("SQL", (a, b, c, d, e) => a + b + c + d + e, queryName: "Test");

            Assert.Equal(esperado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }

        [Fact]
        public void Query_6Tipos_Deve_Chamar_Registrar_Com_Retorno()
        {
            var esperado = new List<string> { "A" };
            servicoTelemetria
                .Setup(t => t.RegistrarComRetorno<string>(It.IsAny<Func<object>>(), "Postgres", "Query Test", "SQL"))
                .Returns(esperado);

            var resultado = connection.Object.Query<string, string, string, string, string, string, string>("SQL", (a, b, c, d, e, f) => a + b + c + d + e + f, queryName: "Test");

            Assert.Equal(esperado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetorno<string>(It.IsAny<Func<object>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }

        [Fact]
        public async Task QueryAsync_6Tipos_Deve_Chamar_Registrar_Com_Retorno_Async()
        {
            var esperado = new List<string> { "A" };
            servicoTelemetria
                .Setup(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"))
                .ReturnsAsync(esperado);

            var resultado = await connection.Object.QueryAsync<string, string, string, string, string, string, string>("SQL", (a, b, c, d, e, f) => a + b + c + d + e + f, queryName: "Test");

            Assert.Equal(esperado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }

        [Fact]
        public void Query_7Tipos_Deve_Chamar_Registrar_Com_Retorno()
        {
            var esperado = new List<string> { "A" };
            servicoTelemetria
                .Setup(t => t.RegistrarComRetorno<string>(It.IsAny<Func<object>>(), "Postgres", "Query Test", "SQL"))
                .Returns(esperado);

            var resultado = connection.Object.Query<string, string, string, string, string, string, string, string>("SQL", (a, b, c, d, e, f, g) => a + b + c + d + e + f + g, queryName: "Test");

            Assert.Equal(esperado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetorno<string>(It.IsAny<Func<object>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }

        [Fact]
        public async Task QueryAsync_7Tipos_Deve_Chamar_Registrar_Com_Retorno_Async()
        {
            var esperado = new List<string> { "A" };
            servicoTelemetria
                .Setup(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"))
                .ReturnsAsync(esperado);

            var resultado = await connection.Object.QueryAsync<string, string, string, string, string, string, string, string>("SQL", (a, b, c, d, e, f, g) => a + b + c + d + e + f + g, queryName: "Test");

            Assert.Equal(esperado, resultado);
            servicoTelemetria.Verify(t => t.RegistrarComRetornoAsync<string>(It.IsAny<Func<Task<object>>>(), "Postgres", "Query Test", "SQL"), Times.Once);
        }
    }
}
