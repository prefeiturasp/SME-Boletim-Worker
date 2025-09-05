using SME.SERAp.Boletim.Infra.Dtos.Elastic;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos.Elastic
{
    public class ResumoGeralProvaDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente_Com_Construtor_Padrao()
        {
            var detalhe = new DetalheProvaDto
            {
                TotalQuestoes = 50,
                Respondidas = 25,
                QtdeQuestoesProva = 50,
                DataInicio = new DateTime(2025, 4, 1),
                DataFim = new DateTime(2025, 4, 10)
            };

            var dto = new ResumoGeralProvaDto
            {
                ProvaId = 1,
                TituloProva = "Prova de Matemática",
                TotalAlunos = 100,
                ProvasIniciadas = 80,
                ProvasNaoFinalizadas = 20,
                ProvasFinalizadas = 60,
                TotalTempoMedio = 600,
                DetalheProva = detalhe,
                TotalTurmas = 3
            };

            Assert.Equal(1, dto.ProvaId);
            Assert.Equal("Prova de Matemática", dto.TituloProva);
            Assert.Equal(100, dto.TotalAlunos);
            Assert.Equal(80, dto.ProvasIniciadas);
            Assert.Equal(20, dto.ProvasNaoFinalizadas);
            Assert.Equal(60, dto.ProvasFinalizadas);
            Assert.Equal(600, dto.TotalTempoMedio);
            Assert.Equal(detalhe, dto.DetalheProva);
            Assert.Equal(3, dto.TotalTurmas);
        }

        [Fact]
        public void Deve_Calcular_PercentualRealizado_Corretamente()
        {
            var dto = new ResumoGeralProvaDto
            {
                TotalAlunos = 50,
                ProvasFinalizadas = 25
            };

            Assert.Equal(50.00m, dto.PercentualRealizado);
        }

        [Fact]
        public void Deve_Retornar_Percentual_Realizado_Zero_Quando_Total_Alunos_Ou_Provas_Finalizadas_Sao_Zero()
        {
            var dto1 = new ResumoGeralProvaDto
            {
                TotalAlunos = 0,
                ProvasFinalizadas = 10
            };

            var dto2 = new ResumoGeralProvaDto
            {
                TotalAlunos = 10,
                ProvasFinalizadas = 0
            };

            Assert.Equal(0m, dto1.PercentualRealizado);
            Assert.Equal(0m, dto2.PercentualRealizado);
        }

        [Fact]
        public void PercentualRealizado_Nao_Deve_Exceder_100()
        {
            var dto = new ResumoGeralProvaDto
            {
                TotalAlunos = 50,
                ProvasFinalizadas = 60
            };

            Assert.Equal(100m, dto.PercentualRealizado);
        }

        [Fact]
        public void Deve_Calcular_TempoMedio_Corretamente()
        {
            var dto = new ResumoGeralProvaDto
            {
                TotalTempoMedio = 600,
                TotalTurmas = 3
            };

            dto.CalcularTempoMedio();

            Assert.Equal(200, dto.TempoMedio);
        }

        [Fact]
        public void Deve_Calcular_Tempo_Medio_Turma_Corretamente()
        {
            var dto = new ResumoGeralProvaDto
            {
                TotalTempoMedio = 600,
                TotalAlunos = 4
            };

            dto.CalcularTempoMedioTurma();

            Assert.Equal(150, dto.TempoMedio);
        }

        [Fact]
        public void Deve_Retornar_Tempo_Medio_Zero_Se_Total_Tempo_Medio_Zero()
        {
            var dto = new ResumoGeralProvaDto
            {
                TotalTempoMedio = 0,
                TotalTurmas = 3
            };

            dto.CalcularTempoMedio();

            Assert.Equal(0, dto.TempoMedio);
        }

        [Fact]
        public void Deve_Retornar_Tempo_Medio_Turma_Zero_Se_Total_Tempo_Medio_Zero()
        {
            var dto = new ResumoGeralProvaDto
            {
                TotalTempoMedio = 0,
                TotalAlunos = 5
            };

            dto.CalcularTempoMedioTurma();

            Assert.Equal(0, dto.TempoMedio);
        }
    }
}
