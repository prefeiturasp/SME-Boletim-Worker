using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos
{
    public class TurmaBoletimResultadoProbabilidadeDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var dto = new TurmaBoletimResultadoProbabilidadeDto
            {
                ProvaId = 1,
                UeId = 2,
                TurmaId = 3,
                TurmaNome = "Turma A",
                AnoLetivo = 2025,
                DisciplinaId = 10,
                AnoEscolar = 5
            };

            Assert.Equal(1, dto.ProvaId);
            Assert.Equal(2, dto.UeId);
            Assert.Equal(3, dto.TurmaId);
            Assert.Equal("Turma A", dto.TurmaNome);
            Assert.Equal(2025, dto.AnoLetivo);
            Assert.Equal(10, dto.DisciplinaId);
            Assert.Equal(5, dto.AnoEscolar);
        }
    }
}
