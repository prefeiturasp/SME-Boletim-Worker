using Dapper;
using Moq;
using Moq.Dapper;
using SME.SERAp.Boletim.Dados.Repositories;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Data;

namespace SME.SERAp.Boletim.Dados.Testes.Repositories
{
    internal class RepositorioAlunoProvaSpProficienciaFake : RepositorioAlunoProvaSpProficiencia
    {
        private readonly IDbConnection _conexaoLeitura;
        private readonly IDbConnection _conexaoEscrita;
        private readonly IDbConnection _conexaoProvaSp;

        public RepositorioAlunoProvaSpProficienciaFake(
            ConnectionStringOptions options,
            IDbConnection conexaoLeitura,
            IDbConnection conexaoEscrita,
            IDbConnection conexaoProvaSp) : base(options)
        {
            _conexaoLeitura = conexaoLeitura;
            _conexaoEscrita = conexaoEscrita;
            _conexaoProvaSp = conexaoProvaSp;
        }

        protected override IDbConnection ObterConexaoLeitura() => _conexaoLeitura;
        protected override IDbConnection ObterConexao() => _conexaoEscrita;
        protected override IDbConnection ObterConexaoProvaSp() => _conexaoProvaSp;
    }

    public class RepositorioAlunoProvaSpProficienciaTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly Mock<IDbConnection> conexaoEscrita;
        private readonly Mock<IDbConnection> conexaoProvaSp;
        private readonly RepositorioAlunoProvaSpProficienciaFake repositorio;

        public RepositorioAlunoProvaSpProficienciaTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            conexaoEscrita = new Mock<IDbConnection>();
            conexaoProvaSp = new Mock<IDbConnection>();

            repositorio = new RepositorioAlunoProvaSpProficienciaFake(
                new ConnectionStringOptions(),
                conexaoLeitura.Object,
                conexaoEscrita.Object,
                conexaoProvaSp.Object
            );
        }

        [Fact]
        public async Task Deve_Obter_Resultado_Aluno_ProvaSp()
        {
            var esperado = new ResultadoAlunoProvaSpDto
            {
                Edicao = "2024",
                AreaConhecimentoID = 2,
                AnoEscolar = "9º",
                AlunoMatricula = "123456",
                Valor = "85.5"
            };

            conexaoProvaSp
                .SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<ResultadoAlunoProvaSpDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterResultadoAlunoProvaSp(2024, 2, "123456");

            Assert.NotNull(resultado);
            Assert.Equal("2024", resultado.Edicao);
            Assert.Equal(2, resultado.AreaConhecimentoID);
            Assert.Equal("123456", resultado.AlunoMatricula);
        }

        [Fact]
        public async Task Deve_Obter_Aluno_ProvaSpProficiencia()
        {
            var esperado = new AlunoProvaSpProficiencia
            {
                Id = 1,
                AlunoRa = 987654,
                AnoEscolar = 9,
                AnoLetivo = 2024,
                DisciplinaId = 3,
                Proficiencia = 75.5m
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<AlunoProvaSpProficiencia>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterAlunoProvaSpProficiencia(2024, 3, 987654);

            Assert.NotNull(resultado);
            Assert.Equal(987654, resultado.AlunoRa);
            Assert.Equal(3, resultado.DisciplinaId);
        }

        [Fact]
        public async Task Deve_Excluir_Aluno_ProvaSpProficiencia()
        {
            conexaoEscrita
                .SetupDapperAsync(c => c.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null, null, null))
                .ReturnsAsync(1);

            var resultado = await repositorio.ExcluirAlunoProvaSpProficiencia(2024, 3, 987654);

            Assert.Equal(1, resultado);
        }
    }
}
