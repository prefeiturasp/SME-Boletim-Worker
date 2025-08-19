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
    internal class RepositorioBoletimProvaAlunoFake : RepositorioBoletimProvaAluno
    {
        private readonly IDbConnection _conexaoLeitura;
        private readonly IDbConnection _conexaoEscrita;

        public RepositorioBoletimProvaAlunoFake(
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

    public class RepositorioBoletimProvaAlunoTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly Mock<IDbConnection> conexaoEscrita;
        private readonly RepositorioBoletimProvaAlunoFake repositorio;
        public RepositorioBoletimProvaAlunoTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            conexaoEscrita = new Mock<IDbConnection>();

            repositorio = new RepositorioBoletimProvaAlunoFake(
                new ConnectionStringOptions(),
                conexaoLeitura.Object,
                conexaoEscrita.Object
            );
        }

        [Fact]
        public async Task Deve_Obter_Boletins_Prova_Alunos_Por_ProvaId_AlunoRa_AnoEscolar()
        {
            var resultadoEsperado = new List<BoletimProvaAluno>
            {
                new BoletimProvaAluno { Id = 1, ProvaId = 123, AlunoRa = 456, AnoEscolar = 5 }
            };

            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<BoletimProvaAluno>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null)).ReturnsAsync(resultadoEsperado);

            var resultado = await repositorio.ObterBoletimProvaAlunoPorProvaIdAlunoRaAnoEscolar(123, 456, 5);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(1, resultado.First().Id);
        }

        [Fact]
        public async Task Deve_Obter_Boletins_Escolares_Detalhes_Por_ProvaId()
        {
            var resultadoEsperado = new List<BoletimEscolarDetalhesDto>
            {
                new BoletimEscolarDetalhesDto { ProvaId = 123, AnoEscolar = 5, Total = 30 }
            };

            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<BoletimEscolarDetalhesDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null)).ReturnsAsync(resultadoEsperado);

            var resultado = await repositorio.ObterBoletinsEscolaresDetalhesPorProvaId(123);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(123, resultado.First().ProvaId);
        }

        [Fact]
        public async Task Deve_Excluir_Boletim_Prova_Aluno_Por_Id_Async()
        {
            conexaoEscrita.SetupDapperAsync(c => c.ExecuteAsync(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null)).ReturnsAsync(1);

            var resultado = await repositorio.ExcluirPorIdAsync(10);

            Assert.Equal(1, resultado);
        }
    }
}
