using Dapper;
using Moq;
using Moq.Dapper;
using SME.SERAp.Boletim.Dados.Repositories;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System.Data;

namespace SME.SERAp.Boletim.Dados.Testes.Repositories
{
    internal class RepositorioBoletimResultadoProbabilidadeFake : RepositorioBoletimResultadoProbabilidade
    {
        private readonly IDbConnection _conexaoLeitura;
        private readonly IDbConnection _conexaoEscrita;

        public RepositorioBoletimResultadoProbabilidadeFake(
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

    public class RepositorioBoletimResultadoProbabilidadeTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly Mock<IDbConnection> conexaoEscrita;
        private readonly RepositorioBoletimResultadoProbabilidadeFake repositorio;
        public RepositorioBoletimResultadoProbabilidadeTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            conexaoEscrita = new Mock<IDbConnection>();

            repositorio = new RepositorioBoletimResultadoProbabilidadeFake(
                new ConnectionStringOptions(),
                conexaoLeitura.Object,
                conexaoEscrita.Object
            );
        }

        [Fact]
        public async Task Deve_Excluir_Boletins_Resultados_Probabilidades_Por_ProvaId_Async()
        {
            conexaoEscrita.SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(1);

            var resultado = await repositorio.ExcluirBoletinsResultadosProbabilidadesPorProvaIdAsync(123);

            Assert.Equal(1, resultado);
        }

        [Fact]
        public async Task Deve_Obter_Turmas_Boletim_Resultado_Probabilidade_Por_ProvaId()
        {
            var resultadoEsperado = new List<TurmaBoletimResultadoProbabilidadeDto>
            {
                new TurmaBoletimResultadoProbabilidadeDto { ProvaId = 123, UeId = 456, TurmaId = 789, TurmaNome = "Turma A" }
            };

            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<TurmaBoletimResultadoProbabilidadeDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await repositorio.ObterTurmasBoletimResultadoProbabilidadePorProvaId(123);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
        }

        [Fact]
        public async Task Deve_Obter_Alunos_Boletim_Resultado_Probabilidade_Por_TurmaId()
        {
            var resultadoEsperado = new List<AlunoBoletimResultadoProbabilidadeDto>
            {
                new AlunoBoletimResultadoProbabilidadeDto { Nome = "Aluno A", Proficiencia = 85.5 }
            };

            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<AlunoBoletimResultadoProbabilidadeDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await repositorio.ObterAlunosBoletimResultadoProbabilidadePorTurmaId(789, 123);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
        }

        [Fact]
        public async Task Deve_Obter_Questoes_Boletim_Resultado_Probabilidade_Por_ProvaId()
        {
            var resultadoEsperado = new List<QuestaoProvaDto>
            {
                new QuestaoProvaDto { DescricaoHabilidade = "Habilidade 1", CodigoHabilidade = "1001" }
            };

            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<QuestaoProvaDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await repositorio.ObterQuestoesBoletimResultaProbabilidadePorProvaId(123);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
        }
    }
}
