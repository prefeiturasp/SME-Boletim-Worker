using Dapper;
using Moq;
using Moq.Dapper;
using SME.SERAp.Boletim.Dados.Repositories;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Data;

namespace SME.SERAp.Boletim.Dados.Testes.Repositories
{
    internal class RepositorioNivelProficienciaFake : RepositorioNivelProficiencia
    {
        private readonly IDbConnection _conexaoLeitura;
        private readonly IDbConnection _conexaoEscrita;

        public RepositorioNivelProficienciaFake(
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

    public class RepositorioNivelProficienciaTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly Mock<IDbConnection> conexaoEscrita;
        private readonly RepositorioNivelProficienciaFake repositorio;

        public RepositorioNivelProficienciaTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            conexaoEscrita = new Mock<IDbConnection>();

            repositorio = new RepositorioNivelProficienciaFake(
                new ConnectionStringOptions(),
                conexaoLeitura.Object,
                conexaoEscrita.Object
            );
        }

        [Fact]
        public async Task Deve_Obter_Niveis_Proficiencia_Por_DisciplinaId_AnoEscolar()
        {
            var esperado = new List<NivelProficiencia>
            {
                new NivelProficiencia
                {
                    Id = 1,
                    Codigo = 1,
                    Descricao = "Básico",
                    ValorReferencia = 200,
                    DisciplinaId = 99,
                    Ano = 5
                }
            };

            conexaoEscrita.SetupDapperAsync(c => c.QueryAsync<NivelProficiencia>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null
            )).ReturnsAsync(esperado);

            var resultado = await repositorio.ObterNiveisProficienciaPorDisciplinaIdAnoEscolar(99, 5);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(1, resultado.FirstOrDefault()!.Codigo);
        }
    }
}
