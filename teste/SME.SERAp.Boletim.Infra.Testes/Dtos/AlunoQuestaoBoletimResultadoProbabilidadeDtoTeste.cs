using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos
{
    public class AlunoQuestaoBoletimResultadoProbabilidadeDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var dto = new AlunoQuestaoBoletimResultadoProbabilidadeDto(
                "HAB1", "Descrição habilidade", 1234, 0.85);

            Assert.Equal("HAB1", dto.CodigoHabilidade);
            Assert.Equal("Descrição habilidade", dto.DescricaoHabilidade);
            Assert.Equal(1234, dto.QuestaoLegadoId);
            Assert.Equal(0.85, dto.Probabilidade);
            Assert.Equal(0, dto.NivelProficiencia); // Valor default do int
        }

        [Fact]
        public void Deve_Modificar_Propriedades_Corretamente()
        {
            var dto = new AlunoQuestaoBoletimResultadoProbabilidadeDto("HAB1", "Descrição", 1, 0.5);

            dto.CodigoHabilidade = "HAB2";
            dto.DescricaoHabilidade = "Nova descrição";
            dto.QuestaoLegadoId = 999;
            dto.Probabilidade = 0.95;
            dto.NivelProficiencia = 3;

            Assert.Equal("HAB2", dto.CodigoHabilidade);
            Assert.Equal("Nova descrição", dto.DescricaoHabilidade);
            Assert.Equal(999, dto.QuestaoLegadoId);
            Assert.Equal(0.95, dto.Probabilidade);
            Assert.Equal(3, dto.NivelProficiencia);
        }
    }
}
