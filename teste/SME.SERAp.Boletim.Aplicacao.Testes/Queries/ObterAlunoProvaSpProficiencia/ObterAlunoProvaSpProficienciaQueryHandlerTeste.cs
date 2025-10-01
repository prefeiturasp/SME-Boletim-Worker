﻿using Moq;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Testes.Queries
{
    public class ObterAlunoProvaSpProficienciaQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_AlunoProvaSpProficiencia()
        {
            var mockRepositorio = new Mock<IRepositorioAlunoProvaSpProficiencia>();
            var anoLetivo = 2023;
            var disciplinaId = 1;
            var alunoRa = 123456;
            var alunoProvaSpProficienciaEsperada = new AlunoProvaSpProficiencia
            {
                Id = 1,
                AlunoRa = alunoRa,
                AnoEscolar = 5,
                AnoLetivo = anoLetivo,
                DisciplinaId = disciplinaId,
                NivelProficiencia = 2,
                Proficiencia = 250.5m,
                DataAtualizacao = DateTime.Now
            };

            mockRepositorio
                .Setup(r => r.ObterAlunoProvaSpProficiencia(anoLetivo, disciplinaId, alunoRa))
                .ReturnsAsync(alunoProvaSpProficienciaEsperada);

            var handler = new ObterAlunoProvaSpProficienciaQueryHandler(mockRepositorio.Object);
            var query = new ObterAlunoProvaSpProficienciaQuery(anoLetivo, disciplinaId, alunoRa);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(alunoProvaSpProficienciaEsperada, resultado);

            mockRepositorio.Verify(r => r.ObterAlunoProvaSpProficiencia(anoLetivo, disciplinaId, alunoRa), Times.Once);
        }
    }
}
