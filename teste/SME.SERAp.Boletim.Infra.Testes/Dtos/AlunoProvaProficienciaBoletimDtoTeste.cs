using SME.SERAp.Boletim.Dominio.Enums;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos
{
    public class AlunoProvaProficienciaBoletimDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var dto = new AlunoProvaProficienciaBoletimDto
            {
                CodigoDre = 100,
                CodigoUe = "UE123",
                NomeUe = "Escola Modelo",
                ProvaId = 10,
                NomeProva = "Prova Final",
                AnoEscolar = 2025,
                Turma = "9º Ano A",
                CodigoAluno = 555,
                NomeAluno = "João Silva",
                NomeDisciplina = "Matemática",
                DisciplinaId = 3,
                ProvaStatus = ProvaStatus.Finalizado,
                Proficiencia = 75.5m,
                ErroMedida = 1.2m,
                NivelCodigo = 2,
                BoletimLoteId = 50
            };

            Assert.Equal(100, dto.CodigoDre);
            Assert.Equal("UE123", dto.CodigoUe);
            Assert.Equal("Escola Modelo", dto.NomeUe);
            Assert.Equal(10, dto.ProvaId);
            Assert.Equal("Prova Final", dto.NomeProva);
            Assert.Equal(2025, dto.AnoEscolar);
            Assert.Equal("9º Ano A", dto.Turma);
            Assert.Equal(555, dto.CodigoAluno);
            Assert.Equal("João Silva", dto.NomeAluno);
            Assert.Equal("Matemática", dto.NomeDisciplina);
            Assert.Equal(3, dto.DisciplinaId);
            Assert.Equal(ProvaStatus.Finalizado, dto.ProvaStatus);
            Assert.Equal(75.5m, dto.Proficiencia);
            Assert.Equal(1.2m, dto.ErroMedida);
            Assert.Equal(2, dto.NivelCodigo);
            Assert.Equal(50, dto.BoletimLoteId);
        }
    }
}
