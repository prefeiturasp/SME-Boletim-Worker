using Dapper;
using Dommel;
using Moq;
using Moq.Dapper;
using SME.SERAp.Boletim.Dados.Repositories;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Data;

namespace SME.SERAp.Boletim.Dados.Testes.Repositories
{
    internal class RepositorioBoletimEscolarFake : RepositorioBoletimEscolar
    {
        private readonly IDbConnection _conexaoLeitura;
        private readonly IDbConnection _conexaoEscrita;

        public RepositorioBoletimEscolarFake(
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

    public class RepositorioBoletimEscolarTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly Mock<IDbConnection> conexaoEscrita;
        private readonly RepositorioBoletimEscolarFake repositorio;

        public RepositorioBoletimEscolarTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            conexaoEscrita = new Mock<IDbConnection>();

            repositorio = new RepositorioBoletimEscolarFake(
                new ConnectionStringOptions(),
                conexaoLeitura.Object,
                conexaoEscrita.Object
            );
        }

        [Fact]
        public async Task Deve_Obter_Boletins_Escolares_Por_ProvaId()
        {
            var provaId = 123;
            var boletins = new List<BoletimEscolar>
            {
                new BoletimEscolar { Id = 1, ProvaId = provaId },
                new BoletimEscolar { Id = 2, ProvaId = provaId }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<BoletimEscolar>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(boletins);

            var resultado = await repositorio.ObterBoletinsEscolaresPorProvaId(provaId);

            Assert.Equal(2, resultado.Count());
            Assert.All(resultado, b => Assert.Equal(provaId, b.ProvaId));
        }

        [Fact]
        public async Task Deve_Excluir_Boletins_Escolares_Por_ProvaId()
        {
            var provaId = 456;
            conexaoEscrita
                .SetupDapperAsync(c => c.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(1);

            var resultado = await repositorio.ExcluirBoletinsEscolaresPorProvaIdAsync(provaId);

            Assert.Equal(1, resultado);
        }
    }
}
