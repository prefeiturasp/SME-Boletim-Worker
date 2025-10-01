using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.Elastic.ObterResumoGeralProvaPorUe;
using SME.SERAp.Boletim.Dados.Interfaces.Elastic;
using SME.SERAp.Boletim.Infra.Dtos.Elastic;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries
{
    public class ObterResumoGeralProvaPorUeQueryHandlerTests
    {
        private readonly Mock<IRepositorioElasticProvaTurmaResultado> _repositorioMock;
        private readonly ObterResumoGeralProvaPorUeQueryHandler _handler;

        public ObterResumoGeralProvaPorUeQueryHandlerTests()
        {
            _repositorioMock = new Mock<IRepositorioElasticProvaTurmaResultado>();
            _handler = new ObterResumoGeralProvaPorUeQueryHandler(_repositorioMock.Object);
        }

        [Fact(DisplayName = "Deve retornar o resumo geral corretamente")]
        public async Task Handle_DeveRetornarResumoGeral_CaminhoFeliz()
        {
            // Arrange
            var query = new ObterResumoGeralProvaPorUeQuery(1, 123, 5);
            var resultadoEsperado = new ResumoGeralProvaDto
            {
                ProvaId = 123,
                TituloProva = "Prova SAEB",
                TotalAlunos = 100,
                ProvasFinalizadas = 80,
                TotalTurmas = 10,
                TotalTempoMedio = 500
            };

            _repositorioMock
                .Setup(r => r.ObterResumoGeralPorUeAsync(query.UeId, query.ProvaId, query.AnoEscolar))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(123, resultado.ProvaId);
            Assert.Equal("Prova SAEB", resultado.TituloProva);
            Assert.Equal(80, resultado.ProvasFinalizadas);
            Assert.Equal(100, resultado.TotalAlunos);
            Assert.Equal(80m, resultado.PercentualRealizado);
            _repositorioMock.Verify(r => r.ObterResumoGeralPorUeAsync(1, 123, 5), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar null se repositório retornar null")]
        public async Task Handle_DeveRetornarNull_SeRepositorioRetornarNull()
        {
            // Arrange
            var query = new ObterResumoGeralProvaPorUeQuery(1, 999, 3);
            _repositorioMock
                .Setup(r => r.ObterResumoGeralPorUeAsync(query.UeId, query.ProvaId, query.AnoEscolar))
                .ReturnsAsync((ResumoGeralProvaDto)null);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(resultado);
        }

        [Theory(DisplayName = "Deve retornar PercentualRealizado como 0 em divisões inválidas")]
        [InlineData(0, 50)] // TotalAlunos = 0
        [InlineData(100, 0)] // ProvasFinalizadas = 0
        public void PercentualRealizado_DeveSerZero_EmCasosInvalidos(long totalAlunos, long provasFinalizadas)
        {
            // Arrange
            var dto = new ResumoGeralProvaDto
            {
                TotalAlunos = totalAlunos,
                ProvasFinalizadas = provasFinalizadas
            };

            // Assert
            Assert.Equal(0, dto.PercentualRealizado);
        }

        [Fact(DisplayName = "Deve limitar PercentualRealizado em 100%")]
        public void PercentualRealizado_NaoDeveUltrapassar100()
        {
            // Arrange
            var dto = new ResumoGeralProvaDto
            {
                TotalAlunos = 50,
                ProvasFinalizadas = 80
            };

            // Act & Assert
            Assert.Equal(100, dto.PercentualRealizado);
        }

        [Fact(DisplayName = "Deve calcular corretamente o tempo médio por turma")]
        public void CalcularTempoMedio_DeveCalcularCorretamente()
        {
            var dto = new ResumoGeralProvaDto
            {
                TotalTempoMedio = 1000,
                TotalTurmas = 10
            };

            dto.CalcularTempoMedio();

            Assert.Equal(100, dto.TempoMedio);
        }

        [Fact(DisplayName = "Deve calcular corretamente o tempo médio por aluno")]
        public void CalcularTempoMedioTurma_DeveCalcularCorretamente()
        {
            var dto = new ResumoGeralProvaDto
            {
                TotalTempoMedio = 800,
                TotalAlunos = 20
            };

            dto.CalcularTempoMedioTurma();

            Assert.Equal(40, dto.TempoMedio);
        }
    }
}