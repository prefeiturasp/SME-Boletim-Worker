using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries
{
    public class ObterUesAlunosRealizaramProvaQueryHandlerTests
    {
        private readonly Mock<IRepositorioBoletimLoteUe> _repositorioMock;
        private readonly ObterUesAlunosRealizaramProvaQueryHandler _handler;

        public ObterUesAlunosRealizaramProvaQueryHandlerTests()
        {
            _repositorioMock = new Mock<IRepositorioBoletimLoteUe>();
            _handler = new ObterUesAlunosRealizaramProvaQueryHandler(_repositorioMock.Object);
        }

        [Fact(DisplayName = "Deve retornar dados corretamente quando repositório retornar resultado")]
        public async Task Handle_DeveRetornarResultado_CaminhoFeliz()
        {
            // Arrange
            var query = new ObterUesAlunosRealizaramProvaQuery(10, 20, 5);

            var resultadoEsperado = new BoletimLoteUeRealizaramProvaDto
            {
                DreId = 1,
                UeId = 20,
                LoteId = 10,
                AnoEscolar = 5,
                RealizaramProva = 42
            };

            _repositorioMock
                .Setup(r => r.ObterUesAlunosRealizaramProva(query.LoteId, query.UeId, query.AnoEscolar))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(resultadoEsperado.DreId, resultado.DreId);
            Assert.Equal(resultadoEsperado.UeId, resultado.UeId);
            Assert.Equal(resultadoEsperado.LoteId, resultado.LoteId);
            Assert.Equal(resultadoEsperado.AnoEscolar, resultado.AnoEscolar);
            Assert.Equal(resultadoEsperado.RealizaramProva, resultado.RealizaramProva);

            _repositorioMock.Verify(r => r.ObterUesAlunosRealizaramProva(query.LoteId, query.UeId, query.AnoEscolar), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar null quando repositório retornar null")]
        public async Task Handle_DeveRetornarNull_SeRepositorioRetornarNull()
        {
            // Arrange
            var query = new ObterUesAlunosRealizaramProvaQuery(99, 88, 7);

            _repositorioMock
                .Setup(r => r.ObterUesAlunosRealizaramProva(query.LoteId, query.UeId, query.AnoEscolar))
                .ReturnsAsync((BoletimLoteUeRealizaramProvaDto)null);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(resultado);

            _repositorioMock.Verify(r => r.ObterUesAlunosRealizaramProva(query.LoteId, query.UeId, query.AnoEscolar), Times.Once);
        }

        [Fact(DisplayName = "Deve lançar exceção se o repositório lançar exceção")]
        public async Task Handle_DeveLancarExcecao_SeRepositorioLancarExcecao()
        {
            // Arrange
            var query = new ObterUesAlunosRealizaramProvaQuery(1, 2, 3);

            _repositorioMock
                .Setup(r => r.ObterUesAlunosRealizaramProva(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro simulado"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact(DisplayName = "Deve tratar corretamente quando parâmetros forem zero")]
        public async Task Handle_DeveTratarParametrosZero()
        {
            // Arrange
            var query = new ObterUesAlunosRealizaramProvaQuery(0, 0, 0);

            var resultadoEsperado = new BoletimLoteUeRealizaramProvaDto
            {
                DreId = 0,
                UeId = 0,
                LoteId = 0,
                AnoEscolar = 0,
                RealizaramProva = 0
            };

            _repositorioMock
                .Setup(r => r.ObterUesAlunosRealizaramProva(0, 0, 0))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.DreId);
            Assert.Equal(0, resultado.UeId);
            Assert.Equal(0, resultado.LoteId);
            Assert.Equal(0, resultado.AnoEscolar);
            Assert.Equal(0, resultado.RealizaramProva);
        }
    }
}