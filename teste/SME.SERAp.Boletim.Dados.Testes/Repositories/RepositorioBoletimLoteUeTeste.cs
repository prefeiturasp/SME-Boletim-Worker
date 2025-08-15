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
    internal class RepositorioBoletimLoteUeFake : RepositorioBoletimLoteUe
    {
        private readonly IDbConnection _conexaoLeitura;
        private readonly IDbConnection _conexaoEscrita;

        public RepositorioBoletimLoteUeFake(
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

    public class RepositorioBoletimLoteUeTeste
    {
        private readonly Mock<IDbConnection> conexaoLeitura;
        private readonly Mock<IDbConnection> conexaoEscrita;
        private readonly RepositorioBoletimLoteUeFake repositorio;
        public RepositorioBoletimLoteUeTeste()
        {
            conexaoLeitura = new Mock<IDbConnection>();
            conexaoEscrita = new Mock<IDbConnection>();

            repositorio = new RepositorioBoletimLoteUeFake(
                new ConnectionStringOptions(),
                conexaoLeitura.Object,
                conexaoEscrita.Object
            );
        }

        [Fact]
        public async Task Deve_Obter_Ues_Total_Alunos_Por_LoteId()
        {
            var esperado = new List<BoletimLoteUe> { new BoletimLoteUe() { Id = 1, AnoEscolar = 5, DreId = 1, LoteId = 1, RealizaramProva = 5, TotalAlunos = 10, UeId = 1 } };
            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<BoletimLoteUe>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterUesTotalAlunosPorLoteId(1);

            Assert.Equivalent(esperado, resultado);
        }

        [Fact]
        public async Task Deve_Obter_Ues_Alunos_Realizaram_Prova_Por_LoteId()
        {
            var esperado = new List<BoletimLoteUeRealizaramProvaDto> { new BoletimLoteUeRealizaramProvaDto() { AnoEscolar = 5, DreId = 1, UeId = 1, RealizaramProva = 5, LoteId = 1 } };
            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<BoletimLoteUeRealizaramProvaDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterUesAlunosRealizaramProvaPorLoteId(1);

            Assert.Equivalent(esperado, resultado);
        }

        [Fact]
        public async Task Deve_Excluir_Boletim_Lote_Ue()
        {
            conexaoEscrita.SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(1);

            var resultado = await repositorio.ExcluirBoletimLoteUe(1, 2, 3);

            Assert.Equal(1, resultado);
        }

        [Fact]
        public async Task Deve_Obter_Ues_Por_Anos_Escolares()
        {
            var esperado = new List<UeDto> { new UeDto() { DreId = 1, Id = 1, Nome = "Teste"} };
            conexaoLeitura.SetupDapperAsync(c => c.QueryAsync<UeDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterUesPorAnosEscolares(new[] { "1" }, 2025);

            Assert.Equivalent(esperado, resultado);
        }

        [Fact]
        public async Task Deve_Obter_Ues_Alunos_Realizaram_Prova()
        {
            var esperado = new BoletimLoteUeRealizaramProvaDto() { AnoEscolar = 5, DreId = 1, UeId = 1, RealizaramProva = 5, LoteId = 1 };
            conexaoLeitura.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<BoletimLoteUeRealizaramProvaDto>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(esperado);

            var resultado = await repositorio.ObterUesAlunosRealizaramProva(1, 2, 3);

            Assert.Equivalent(esperado, resultado);
        }
    }
}
