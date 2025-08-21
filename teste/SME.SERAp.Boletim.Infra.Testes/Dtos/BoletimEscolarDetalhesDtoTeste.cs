using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos
{
    public class BoletimEscolarDetalhesDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var dto = new BoletimEscolarDetalhesDto
            {
                UeId = 1,
                ProvaId = 2,
                AnoEscolar = 3,
                Disciplina = "Matemática",
                DisciplinaId = 4,
                AbaixoBasico = 10.5m,
                AbaixoBasicoPorcentagem = 20.5m,
                Basico = 30.5m,
                BasicoPorcentagem = 40.5m,
                Adequado = 50.5m,
                AdequadoPorcentagem = 60.5m,
                Avancado = 70.5m,
                AvancadoPorcentagem = 80.5m,
                Total = 100,
                MediaProficiencia = 7.5m,
                NivelUeCodigo = 123,
                NivelUeDescricao = "Ensino Fundamental"
            };

            Assert.Equal(1, dto.UeId);
            Assert.Equal(2, dto.ProvaId);
            Assert.Equal(3, dto.AnoEscolar);
            Assert.Equal("Matemática", dto.Disciplina);
            Assert.Equal(4, dto.DisciplinaId);
            Assert.Equal(10.5m, dto.AbaixoBasico);
            Assert.Equal(20.5m, dto.AbaixoBasicoPorcentagem);
            Assert.Equal(30.5m, dto.Basico);
            Assert.Equal(40.5m, dto.BasicoPorcentagem);
            Assert.Equal(50.5m, dto.Adequado);
            Assert.Equal(60.5m, dto.AdequadoPorcentagem);
            Assert.Equal(70.5m, dto.Avancado);
            Assert.Equal(80.5m, dto.AvancadoPorcentagem);
            Assert.Equal(100, dto.Total);
            Assert.Equal(7.5m, dto.MediaProficiencia);
            Assert.Equal(123, dto.NivelUeCodigo);
            Assert.Equal("Ensino Fundamental", dto.NivelUeDescricao);
        }

        [Fact]
        public void Deve_Retornar_ComponenteCurricularCorreto()
        {
            var dto = new BoletimEscolarDetalhesDto
            {
                Disciplina = "Matemática",
                AnoEscolar = 3
            };

            var resultado = dto.ComponenteCurricular;

            Assert.Equal("Matemática (3º ano)", resultado);
        }
    }
}
