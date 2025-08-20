using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos
{
    public class AlunoBoletimResultadoProbabilidadeDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var dto = new AlunoBoletimResultadoProbabilidadeDto
            {
                Id = 1,
                Nome = "João Silva",
                Codigo = 123,
                TurmaId = 10,
                TurmaNome = "Turma A",
                UeId = 100,
                Proficiencia = 75.5,
                NivelProficiencia = 3
            };

            Assert.Equal(1, dto.Id);
            Assert.Equal("João Silva", dto.Nome);
            Assert.Equal(123, dto.Codigo);
            Assert.Equal(10, dto.TurmaId);
            Assert.Equal("Turma A", dto.TurmaNome);
            Assert.Equal(100, dto.UeId);
            Assert.Equal(75.5, dto.Proficiencia);
            Assert.Equal(3, dto.NivelProficiencia);
        }

        [Fact]
        public void Deve_Inicializar_Lista_Se_ForNula()
        {
            var dto = new AlunoBoletimResultadoProbabilidadeDto();

            dto.AdicionarQuestao("HAB1", "Descrição habilidade", 1234, 0.85);

            Assert.NotNull(dto.Questoes);
            Assert.Single(dto.Questoes);
        }

        [Fact]
        public void Deve_Adicionar_Questao_Com_Valores_Corretos()
        {
            var dto = new AlunoBoletimResultadoProbabilidadeDto();

            dto.AdicionarQuestao("HAB2", "Outra habilidade", 5678, 0.65);

            var questao = Assert.Single(dto.Questoes);
            Assert.Equal("HAB2", questao.CodigoHabilidade);
            Assert.Equal("Outra habilidade", questao.DescricaoHabilidade);
            Assert.Equal(5678, questao.QuestaoLegadoId);
            Assert.Equal(0.65, questao.Probabilidade);
        }

        [Fact]
        public void Deve_Adicionar_Multiplas_Questoes()
        {
            var dto = new AlunoBoletimResultadoProbabilidadeDto();

            dto.AdicionarQuestao("HAB1", "Descrição 1", 111, 0.5);
            dto.AdicionarQuestao("HAB2", "Descrição 2", 222, 0.7);

            Assert.Equal(2, dto.Questoes.Count);
            Assert.Collection(dto.Questoes,
                q =>
                {
                    Assert.Equal("HAB1", q.CodigoHabilidade);
                    Assert.Equal("Descrição 1", q.DescricaoHabilidade);
                    Assert.Equal(111, q.QuestaoLegadoId);
                    Assert.Equal(0.5, q.Probabilidade);
                },
                q =>
                {
                    Assert.Equal("HAB2", q.CodigoHabilidade);
                    Assert.Equal("Descrição 2", q.DescricaoHabilidade);
                    Assert.Equal(222, q.QuestaoLegadoId);
                    Assert.Equal(0.7, q.Probabilidade);
                });
        }
    }
}
