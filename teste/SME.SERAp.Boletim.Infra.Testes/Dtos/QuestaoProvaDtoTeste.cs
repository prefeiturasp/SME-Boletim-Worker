using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos
{
    public class QuestaoProvaDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var probabilidades = new Dictionary<int, double>
            {
                { 1, 0.25 },
                { 2, 0.50 },
                { 3, 0.75 }
            };

            var dto = new QuestaoProvaDto
            {
                DescricaoHabilidade = "Interpretação de texto",
                CodigoHabilidade = "HAB123",
                HabilidadeId = 10,
                QuestaoLegadoId = 99,
                Discriminacao = 1.2,
                Dificuldade = 0.8,
                AcertoCasual = 0.2,
                Probabilidades = probabilidades
            };

            Assert.Equal("Interpretação de texto", dto.DescricaoHabilidade);
            Assert.Equal("HAB123", dto.CodigoHabilidade);
            Assert.Equal(10, dto.HabilidadeId);
            Assert.Equal(99, dto.QuestaoLegadoId);
            Assert.Equal(1.2, dto.Discriminacao);
            Assert.Equal(0.8, dto.Dificuldade);
            Assert.Equal(0.2, dto.AcertoCasual);
            Assert.Equal(probabilidades, dto.Probabilidades);
        }
    }
}
