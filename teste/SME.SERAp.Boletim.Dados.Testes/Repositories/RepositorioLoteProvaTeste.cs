using Dapper;
using Moq;
using Moq.Dapper;
using SME.SERAp.Boletim.Dados.Repositories;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Dominio.Enums;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Data;

namespace SME.SERAp.Boletim.Dados.Testes.Repositories
{
    internal class RepositorioLoteProvaFake : RepositorioLoteProva
    {
        private readonly IDbConnection _conexaoLeitura;
        private readonly IDbConnection _conexaoEscrita;

        public RepositorioLoteProvaFake(
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

    public class RepositorioLoteProvaTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly Mock<IDbConnection> conexaoEscrita;
        private readonly RepositorioLoteProvaFake repositorio;

        public RepositorioLoteProvaTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            conexaoEscrita = new Mock<IDbConnection>();

            repositorio = new RepositorioLoteProvaFake(
                new ConnectionStringOptions(),
                conexaoLeitura.Object,
                conexaoEscrita.Object
            );
        }

        [Fact]
        public async Task Deve_Desativar_Todos_Lotes_Prova_Ativos()
        {
            conexaoEscrita.SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(1);

            var resultado = await repositorio.DesativarTodosLotesProvaAtivos();

            Assert.Equal(1, resultado);
        }

        [Fact]
        public async Task Deve_Obter_Lotes_Prova_Por_Data()
        {
            var esperado = new List<LoteProva>
            {
                new LoteProva { Id = 1, Nome = "Lote Teste" }
            };

            conexaoEscrita.SetupDapperAsync(c => c.QueryAsync<LoteProva>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null
            )).ReturnsAsync(esperado);

            var resultado = await repositorio.ObterLotesProvaPorData(DateTime.Now.AddDays(-1), DateTime.Now, true);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Lote Teste", resultado.FirstOrDefault()!.Nome);
        }

        [Fact]
        public async Task Deve_Obter_Resultado_Ao_Alterar_Status_Consolidacao()
        {
            conexaoEscrita.SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(1);

            var resultado = await repositorio.AlterarStatusConsolidacao(1, LoteStatusConsolidacao.Pendente);

            Assert.Equal(1, resultado);
        }

        [Fact]
        public async Task Deve_Obter_Provas_Tai_Ano_Por_LoteId()
        {
            var esperado = new List<ProvaDto>
            {
                new ProvaDto { Id = 10, Descricao = "Prova Teste" }
            };

            conexaoEscrita.SetupDapperAsync(c => c.QueryAsync<ProvaDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null
            )).ReturnsAsync(esperado);

            var resultado = await repositorio.ObterProvasTaiAnoPorLoteId(1);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Prova Teste", resultado.FirstOrDefault()!.Descricao);
        }
    }
}
