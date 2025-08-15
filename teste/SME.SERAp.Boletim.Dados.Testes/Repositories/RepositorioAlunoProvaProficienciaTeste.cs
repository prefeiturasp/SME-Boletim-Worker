using Dapper;
using Moq;
using Moq.Dapper;
using SME.SERAp.Boletim.Dados.Repositories;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Data;

namespace SME.SERAp.Boletim.Dados.Testes.Repositories
{
    internal class RepositorioAlunoProvaProficienciaFake : RepositorioAlunoProvaProficiencia
    {
        private readonly IDbConnection _conexaoLeitura;
        private readonly IDbConnection _conexaoEscrita;

        public RepositorioAlunoProvaProficienciaFake(
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

    public class RepositorioAlunoProvaProficienciaTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly Mock<IDbConnection> conexaoEscrita;
        private readonly RepositorioAlunoProvaProficienciaFake repositorio;

        public RepositorioAlunoProvaProficienciaTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            conexaoEscrita = new Mock<IDbConnection>();

            repositorio = new RepositorioAlunoProvaProficienciaFake(
                new ConnectionStringOptions(),
                conexaoLeitura.Object,
                conexaoEscrita.Object
            );
        }

        [Fact]
        public async Task Deve_Obter_Alunos_Prova_Proficiencia_Boletim_Por_ProvaId()
        {
            var provaId = 123;
            var alunos = new List<AlunoProvaProficienciaBoletimDto>
            {
                new AlunoProvaProficienciaBoletimDto { ProvaId = provaId, NomeAluno = "Aluno 1" },
                new AlunoProvaProficienciaBoletimDto { ProvaId = provaId, NomeAluno = "Aluno 2" }
            };

            conexaoLeitura
                .SetupDapperAsync(c => c.QueryAsync<AlunoProvaProficienciaBoletimDto>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    null))
                .ReturnsAsync(alunos);

            var resultado = await repositorio.ObterAlunosProvaProficienciaBoletimPorProvaId(provaId);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.All(resultado, a => Assert.Equal(provaId, a.ProvaId));
        }
    }
}
