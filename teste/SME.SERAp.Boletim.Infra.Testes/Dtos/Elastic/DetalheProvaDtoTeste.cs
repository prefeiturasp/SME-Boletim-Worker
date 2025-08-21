using SME.SERAp.Boletim.Infra.Dtos.Elastic;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos.Elastic
{
    public class DetalheProvaDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente_Com_Construtor_Padrao()
        {
            var inicio = new DateTime(2025, 3, 1);
            var fim = new DateTime(2025, 3, 10);

            var dto = new DetalheProvaDto
            {
                DataInicio = inicio,
                DataFim = fim,
                QtdeQuestoesProva = 50,
                TotalQuestoes = 50,
                Respondidas = 25
            };

            Assert.Equal(inicio, dto.DataInicio);
            Assert.Equal(fim, dto.DataFim);
            Assert.Equal(50, dto.QtdeQuestoesProva);
            Assert.Equal(50, dto.TotalQuestoes);
            Assert.Equal(25, dto.Respondidas);
        }

        [Fact]
        public void Deve_Calcular_Percentual_Respondido_Corretamente()
        {
            var dto = new DetalheProvaDto
            {
                TotalQuestoes = 40,
                Respondidas = 20
            };

            Assert.Equal(50.00m, dto.PercentualRespondido);
        }

        [Fact]
        public void Deve_Retornar_Percentual_Respondido_Zero_Quando_Total_Questoes_Ou_Respondidas_Sao_Zero()
        {
            var dto1 = new DetalheProvaDto
            {
                TotalQuestoes = 0,
                Respondidas = 10
            };

            var dto2 = new DetalheProvaDto
            {
                TotalQuestoes = 10,
                Respondidas = 0
            };

            Assert.Equal(0m, dto1.PercentualRespondido);
            Assert.Equal(0m, dto2.PercentualRespondido);
        }

        [Fact]
        public void PercentualRespondido_Nao_Deve_Exceder_100()
        {
            var dto = new DetalheProvaDto
            {
                TotalQuestoes = 50,
                Respondidas = 60
            };

            Assert.Equal(100m, dto.PercentualRespondido);
        }
    }
}
