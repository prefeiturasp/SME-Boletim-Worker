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

        [Fact]
        public async Task Deve_Obter_Boletim_Prova_Aluno_Ultima_Turma_Por_AnoEscolar()
        {
            var resultadoEsperado = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 111L,
                CodigoUe = "UE001",
                NomeUe = "Escola A",
                AnoEscolar = 5,
                Turma = "5A",
                NivelCodigo = 3
            };

            conexaoEscrita.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<BoletimProvaAlunoUltimaTurmaAlunoDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null)).ReturnsAsync(resultadoEsperado);

            var resultado = await repositorio.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(123456L, 2024, 5, 789L, 75.5m);

            Assert.NotNull(resultado);
            Assert.Equal(111L, resultado.CodigoDre);
            Assert.Equal("UE001", resultado.CodigoUe);
            Assert.Equal("Escola A", resultado.NomeUe);
            Assert.Equal(5, resultado.AnoEscolar);
            Assert.Equal("5A", resultado.Turma);
            Assert.Equal(3, resultado.NivelCodigo);
        }

        [Fact]
        public async Task Deve_Obter_Boletim_Prova_Aluno_Ultima_Turma_Nulo()
        {
            conexaoEscrita.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<BoletimProvaAlunoUltimaTurmaAlunoDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null)).ReturnsAsync((BoletimProvaAlunoUltimaTurmaAlunoDto)null);

            var resultado = await repositorio.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(999999L, 2024, 8, 456L, 50.0m);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Deve_Obter_Boletim_Prova_Aluno_Ultima_Turma_Com_Diferentes_AnoEscolar()
        {
            var resultadoAnoEscolar5 = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 111L,
                AnoEscolar = 5,
                NivelCodigo = 2
            };

            var resultadoAnoEscolar6 = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 222L,
                AnoEscolar = 6,
                NivelCodigo = 3
            };

            conexaoEscrita.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<BoletimProvaAlunoUltimaTurmaAlunoDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null)).ReturnsAsync(resultadoAnoEscolar5);

            var resultado1 = await repositorio.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(123456L, 2024, 5, 789L, 75.5m);

            conexaoEscrita.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<BoletimProvaAlunoUltimaTurmaAlunoDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null)).ReturnsAsync(resultadoAnoEscolar6);

            var resultado2 = await repositorio.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(123456L, 2024, 6, 789L, 75.5m);

            Assert.NotNull(resultado1);
            Assert.NotNull(resultado2);
            Assert.NotEqual(resultado1.AnoEscolar, resultado2.AnoEscolar);
            Assert.NotEqual(resultado1.CodigoDre, resultado2.CodigoDre);
        }

        [Fact]
        public async Task Deve_Obter_Boletim_Prova_Aluno_Ultima_Turma_Com_Proficiencia_Alta()
        {
            var resultadoEsperado = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 333L,
                CodigoUe = "UE003",
                NomeUe = "Escola C",
                AnoEscolar = 9,
                Turma = "9A",
                NivelCodigo = 5
            };

            conexaoEscrita.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<BoletimProvaAlunoUltimaTurmaAlunoDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null)).ReturnsAsync(resultadoEsperado);

            var resultado = await repositorio.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(654321L, 2025, 9, 123L, 95.5m);

            Assert.NotNull(resultado);
            Assert.Equal(5, resultado.NivelCodigo);
        }

        [Fact]
        public async Task Deve_Obter_Boletim_Prova_Aluno_Ultima_Turma_Com_Proficiencia_Baixa()
        {
            var resultadoEsperado = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 444L,
                CodigoUe = "UE004",
                NomeUe = "Escola D",
                AnoEscolar = 7,
                Turma = "7B",
                NivelCodigo = 1
            };

            conexaoEscrita.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<BoletimProvaAlunoUltimaTurmaAlunoDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null)).ReturnsAsync(resultadoEsperado);

            var resultado = await repositorio.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(111111L, 2024, 7, 456L, 40.0m);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.NivelCodigo);
        }

        [Fact]
        public async Task Deve_Obter_Boletim_Prova_Aluno_Com_Todos_Os_Parametros()
        {
            var alunRa = 123456L;
            var anoLetivo = 2024;
            var anoEscolar = 5;
            var disciplinaId = 789L;
            var proficiencia = 75.5m;

            var resultadoEsperado = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 111L,
                CodigoUe = "UE001",
                NomeUe = "Escola A",
                AnoEscolar = anoEscolar,
                Turma = "5A",
                NivelCodigo = 3
            };

            conexaoEscrita.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<BoletimProvaAlunoUltimaTurmaAlunoDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null)).ReturnsAsync(resultadoEsperado);

            var resultado = await repositorio.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia);

            Assert.NotNull(resultado);
            Assert.Equal(anoEscolar, resultado.AnoEscolar);
        }
    }
}
