using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar
{
    public class ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Boletim_Prova_Aluno_Ultima_Turma()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimProvaAluno>();

            var alunRa = 123456L;
            var anoLetivo = 2024;
            var anoEscolar = 5;
            var disciplinaId = 789L;
            var proficiencia = 75.5m;

            var boletimEsperado = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 111L,
                CodigoUe = "UE001",
                NomeUe = "Escola A",
                AnoEscolar = anoEscolar,
                Turma = "5A",
                NivelCodigo = 3
            };

            mockRepositorio
                .Setup(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia))
                .ReturnsAsync(boletimEsperado);

            var handler = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQueryHandler(mockRepositorio.Object);
            var query = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(boletimEsperado.CodigoDre, resultado.CodigoDre);
            Assert.Equal(boletimEsperado.CodigoUe, resultado.CodigoUe);
            Assert.Equal(boletimEsperado.NomeUe, resultado.NomeUe);
            Assert.Equal(boletimEsperado.AnoEscolar, resultado.AnoEscolar);
            Assert.Equal(boletimEsperado.Turma, resultado.Turma);
            Assert.Equal(boletimEsperado.NivelCodigo, resultado.NivelCodigo);

            mockRepositorio.Verify(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia), Times.Once);
        }

        [Fact]
        public async Task Deve_Chamar_Repositorio_Com_Parametros_Corretos()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimProvaAluno>();

            var alunRa = 654321L;
            var anoLetivo = 2023;
            var anoEscolar = 6;
            var disciplinaId = 456L;
            var proficiencia = 85.0m;

            var boletim = new BoletimProvaAlunoUltimaTurmaAlunoDto();

            mockRepositorio
                .Setup(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<decimal>()))
                .ReturnsAsync(boletim);

            var handler = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQueryHandler(mockRepositorio.Object);
            var query = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia);

            await handler.Handle(query, CancellationToken.None);

            mockRepositorio.Verify(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Null_Quando_Repositorio_Retorna_Null()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimProvaAluno>();

            var alunRa = 999999L;
            var anoLetivo = 2022;
            var anoEscolar = 4;
            var disciplinaId = 321L;
            var proficiencia = 60.0m;

            mockRepositorio
                .Setup(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia))
                .ReturnsAsync((BoletimProvaAlunoUltimaTurmaAlunoDto)null);

            var handler = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQueryHandler(mockRepositorio.Object);
            var query = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Null(resultado);

            mockRepositorio.Verify(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Chamar_Repositorio_Mais_De_Uma_Vez()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimProvaAluno>();

            var alunRa = 111111L;
            var anoLetivo = 2025;
            var anoEscolar = 7;
            var disciplinaId = 555L;
            var proficiencia = 90.5m;

            var boletim = new BoletimProvaAlunoUltimaTurmaAlunoDto();

            mockRepositorio
                .Setup(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<decimal>()))
                .ReturnsAsync(boletim);

            var handler = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQueryHandler(mockRepositorio.Object);
            var query = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia);

            await handler.Handle(query, CancellationToken.None);

            mockRepositorio.Verify(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<decimal>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Propagar_Excecao_Do_Repositorio()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimProvaAluno>();

            var alunRa = 222222L;
            var anoLetivo = 2024;
            var anoEscolar = 5;
            var disciplinaId = 777L;
            var proficiencia = 70.0m;

            mockRepositorio
                .Setup(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia))
                .ThrowsAsync(new InvalidOperationException("Erro ao acessar repositório"));

            var handler = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQueryHandler(mockRepositorio.Object);
            var query = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(query, CancellationToken.None));

            mockRepositorio.Verify(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Dto_Com_Dados_Corretos()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimProvaAluno>();

            var alunRa = 333333L;
            var anoLetivo = 2024;
            var anoEscolar = 8;
            var disciplinaId = 999L;
            var proficiencia = 95.5m;

            var boletimEsperado = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 555L,
                CodigoUe = "UE002",
                NomeUe = "Escola B",
                AnoEscolar = 8,
                Turma = "8B",
                NivelCodigo = 4
            };

            mockRepositorio
                .Setup(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia))
                .ReturnsAsync(boletimEsperado);

            var handler = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQueryHandler(mockRepositorio.Object);
            var query = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(555L, resultado.CodigoDre);
            Assert.Equal("UE002", resultado.CodigoUe);
            Assert.Equal("Escola B", resultado.NomeUe);
            Assert.Equal(8, resultado.AnoEscolar);
            Assert.Equal("8B", resultado.Turma);
            Assert.Equal(4, resultado.NivelCodigo);
        }

        [Fact]
        public async Task Deve_Retornar_Diferentes_Boletins_Para_Diferentes_Parametros()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimProvaAluno>();

            var alunRa1 = 444444L;
            var alunRa2 = 555555L;
            var anoLetivo = 2024;
            var anoEscolar = 5;
            var disciplinaId = 111L;
            var proficiencia = 80.0m;

            var boletim1 = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 111L,
                CodigoUe = "UE001",
                NomeUe = "Escola A",
                AnoEscolar = 5,
                Turma = "5A",
                NivelCodigo = 2
            };

            var boletim2 = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 222L,
                CodigoUe = "UE002",
                NomeUe = "Escola C",
                AnoEscolar = 5,
                Turma = "5B",
                NivelCodigo = 3
            };

            mockRepositorio
                .Setup(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa1, anoLetivo, anoEscolar, disciplinaId, proficiencia))
                .ReturnsAsync(boletim1);

            mockRepositorio
                .Setup(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa2, anoLetivo, anoEscolar, disciplinaId, proficiencia))
                .ReturnsAsync(boletim2);

            var handler = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQueryHandler(mockRepositorio.Object);
            var query1 = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery(alunRa1, anoLetivo, anoEscolar, disciplinaId, proficiencia);
            var query2 = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery(alunRa2, anoLetivo, anoEscolar, disciplinaId, proficiencia);

            var resultado1 = await handler.Handle(query1, CancellationToken.None);
            var resultado2 = await handler.Handle(query2, CancellationToken.None);

            Assert.NotEqual(resultado1.CodigoDre, resultado2.CodigoDre);
            Assert.NotEqual(resultado1.CodigoUe, resultado2.CodigoUe);
            Assert.Equal(resultado1.AnoEscolar, resultado2.AnoEscolar);

            mockRepositorio.Verify(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa1, anoLetivo, anoEscolar, disciplinaId, proficiencia), Times.Once);
            mockRepositorio.Verify(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa2, anoLetivo, anoEscolar, disciplinaId, proficiencia), Times.Once);
        }

        [Fact]
        public void Constructor_Deve_Aceitar_Repositorio()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimProvaAluno>();

            var handler = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQueryHandler(mockRepositorio.Object);

            Assert.NotNull(handler);
        }

        [Fact]
        public async Task Deve_Respeitar_Valores_De_Proficiencia()
        {
            var mockRepositorio = new Mock<IRepositorioBoletimProvaAluno>();

            var alunRa = 666666L;
            var anoLetivo = 2024;
            var anoEscolar = 5;
            var disciplinaId = 222L;
            var proficiencia1 = 50.0m;
            var proficiencia2 = 100.0m;

            var boletim1 = new BoletimProvaAlunoUltimaTurmaAlunoDto { NivelCodigo = 1 };
            var boletim2 = new BoletimProvaAlunoUltimaTurmaAlunoDto { NivelCodigo = 5 };

            mockRepositorio
                .Setup(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia1))
                .ReturnsAsync(boletim1);

            mockRepositorio
                .Setup(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia2))
                .ReturnsAsync(boletim2);

            var handler = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQueryHandler(mockRepositorio.Object);
            var query1 = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia1);
            var query2 = new ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolarQuery(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia2);

            var resultado1 = await handler.Handle(query1, CancellationToken.None);
            var resultado2 = await handler.Handle(query2, CancellationToken.None);

            Assert.Equal(1, resultado1.NivelCodigo);
            Assert.Equal(5, resultado2.NivelCodigo);

            mockRepositorio.Verify(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia1), Times.Once);
            mockRepositorio.Verify(r => r.ObterBoletimProvaAlunoUltimaTurmaAlunoPorAnoEscolar(alunRa, anoLetivo, anoEscolar, disciplinaId, proficiencia2), Times.Once);
        }
    }
}
