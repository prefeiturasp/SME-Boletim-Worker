using Dapper;
using Moq;
using Moq.Dapper;
using SME.SERAp.Boletim.Dados.Repositories;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Data;

namespace SME.SERAp.Boletim.Dados.Testes.Repositories
{
    internal class RepositorioProvaFake : RepositorioProva
    {
        private readonly IDbConnection _conexaoLeitura;
        private readonly IDbConnection _conexaoEscrita;

        public RepositorioProvaFake(
            ConnectionStringOptions options,
            IDbConnection conexaoLeitura,
            IDbConnection conexaoEscrita) : base(options)
        {
            _conexaoLeitura = conexaoLeitura;
            _conexaoEscrita = conexaoEscrita;
        }

        protected override IDbConnection ObterConexaoLeitura() => _conexaoLeitura;
        protected override IDbConnection ObterConexao() => _conexaoEscrita;
    }

    public class RepositorioProvaTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly Mock<IDbConnection> conexaoEscrita;
        private readonly RepositorioProvaFake repositorio;

        public RepositorioProvaTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            conexaoEscrita = new Mock<IDbConnection>();

            repositorio = new RepositorioProvaFake(
                new ConnectionStringOptions(),
                conexaoLeitura.Object,
                conexaoEscrita.Object
            );
        }

        [Fact]
        public async Task Deve_Obter_Provas_Finalizadas_Por_Data()
        {
            var esperado = new List<ProvaDto>
            {
                new ProvaDto
                {
                    Id = 1,
                    Codigo = 123,
                    Descricao = "Prova Matemática",
                    Modalidade = Dominio.Enums.Modalidade.Fundamental,
                    Inicio = DateTime.Now.AddDays(-10),
                    Fim = DateTime.Now.AddDays(-5),
                    DataCorrecaoInicio = DateTime.Now.AddDays(-4),
                    DataCorrecaoFim = DateTime.Today,
                    ExibirNoBoletim = true,
                    FormatoTai = true
                }
            };

            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<ProvaDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null
            )).ReturnsAsync(esperado);

            var resultado = await repositorio.ObterProvasFinalizadasPorData(DateTime.Today);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Prova Matemática", resultado.FirstOrDefault()!.Descricao);
        }

        [Fact]
        public async Task Deve_Obter_Ano_Prova()
        {
            var esperado = 2023;

            conexaoLeitura.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<int>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null
            )).ReturnsAsync(esperado);

            var resultado = await repositorio.ObterAnoProva(1);
            Assert.NotNull(resultado);
            Assert.Equal(2023, resultado);
        }

        [Fact]
        public async Task Deve_Obter_Ano_Prova_Nulo()
        {
            conexaoLeitura.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<int?>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null
            )).ReturnsAsync((int?)null);

            var resultado = await repositorio.ObterAnoProva(1);
            Assert.Null(resultado);
        }
    }
}
