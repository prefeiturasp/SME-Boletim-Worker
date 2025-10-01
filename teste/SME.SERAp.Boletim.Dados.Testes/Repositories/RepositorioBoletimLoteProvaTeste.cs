using Dapper;
using Moq;
using Moq.Dapper;
using SME.SERAp.Boletim.Dados.Repositories;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Data;

namespace SME.SERAp.Boletim.Dados.Testes.Repositories
{
    internal class RepositorioBoletimLoteProvaFake : RepositorioBoletimLoteProva
    {
        private readonly IDbConnection _conexaoLeitura;
        private readonly IDbConnection _conexaoEscrita;

        public RepositorioBoletimLoteProvaFake(
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

    public class RepositorioBoletimLoteProvaTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly Mock<IDbConnection> conexaoEscrita;
        private readonly RepositorioBoletimLoteProvaFake repositorio;

        public RepositorioBoletimLoteProvaTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            conexaoEscrita = new Mock<IDbConnection>();

            repositorio = new RepositorioBoletimLoteProvaFake(
                new ConnectionStringOptions(),
                conexaoLeitura.Object,
                conexaoEscrita.Object
            );
        }

        [Fact]
        public async Task Deve_Obter_Boletim_Lote_Prova_Pendentes()
        {
            var provaId = 123;
            var esperado = new List<BoletimLoteProva>
            {
                new BoletimLoteProva { Id = 1, LoteId = 10, ProvaId = provaId }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<BoletimLoteProva>(
                    It.IsAny<string>(),
                    null,
                    null,
                    null,
                    null))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterBoletimLoteProvaPendentes();

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.All(resultado, b => Assert.Equal(provaId, b.ProvaId));
        }

        [Fact]
        public async Task Deve_Fechar_Conexao_Apos_Consulta()
        {
            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<BoletimLoteProva>(
                    It.IsAny<string>(),
                    null,
                    null,
                    null,
                    null))
                .ReturnsAsync(new List<BoletimLoteProva>());

            await repositorio.ObterBoletimLoteProvaPendentes();

            conexaoLeitura.Verify(c => c.Close(), Times.Once);
        }
    }
}
