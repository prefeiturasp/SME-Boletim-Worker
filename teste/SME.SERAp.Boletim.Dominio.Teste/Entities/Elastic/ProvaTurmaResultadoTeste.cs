using SME.SERAp.Boletim.Dominio.Entities.Elastic;
using SME.SERAp.Boletim.Dominio.Enums;

namespace SME.SERAp.Boletim.Dominio.Teste.Entities.Elastic
{
    public class ProvaTurmaResultadoTeste
    {
        [Fact]
        public void Construtor_DeveAtribuirPropriedadesCorretamente()
        {
            var provaId = 1L;
            var dreId = 2L;
            var ueId = 3L;
            var turmaId = 4L;
            var ano = "2025";
            var modalidade = Modalidade.Fundamental;
            var anoLetivo = 2025;
            var inicio = new DateTime(2025, 1, 10);
            var fim = new DateTime(2025, 1, 20);
            var descricao = "Prova de Matemática";
            var totalAlunos = 30L;
            var totalIniciadas = 25L;
            var totalNaoFinalizados = 5L;
            var totalFinalizados = 20L;
            var quantidadeQuestoes = 10L;
            var totalQuestoes = 10L;
            var questoesRespondidas = 8L;
            var tempoMedio = 35L;

            var resultado = new ProvaTurmaResultado(
                provaId, dreId, ueId, turmaId, ano, modalidade, anoLetivo,
                inicio, fim, descricao, totalAlunos, totalIniciadas,
                totalNaoFinalizados, totalFinalizados, quantidadeQuestoes,
                totalQuestoes, questoesRespondidas, tempoMedio
            );

            Assert.Equal(provaId, resultado.ProvaId);
            Assert.Equal(dreId, resultado.DreId);
            Assert.Equal(ueId, resultado.UeId);
            Assert.Equal(turmaId, resultado.TurmaId);
            Assert.Equal(ano, resultado.Ano);
            Assert.Equal(modalidade, resultado.Modalidade);
            Assert.Equal(anoLetivo, resultado.AnoLetivo);
            Assert.Equal(inicio, resultado.Inicio);
            Assert.Equal(fim, resultado.Fim);
            Assert.Equal(descricao, resultado.Descricao);
            Assert.Equal(totalAlunos, resultado.TotalAlunos);
            Assert.Equal(totalIniciadas, resultado.TotalIniciadas);
            Assert.Equal(totalNaoFinalizados, resultado.TotalNaoFinalizados);
            Assert.Equal(totalFinalizados, resultado.TotalFinalizados);
            Assert.Equal(quantidadeQuestoes, resultado.QuantidadeQuestoes);
            Assert.Equal(totalQuestoes, resultado.TotalQuestoes);
            Assert.Equal(questoesRespondidas, resultado.QuestoesRespondidas);
            Assert.Equal(tempoMedio, resultado.TempoMedio);
        }

        [Fact]
        public void DevePermitirAlterarPropriedades()
        {
            var resultado = new ProvaTurmaResultado(
                1, 2, 3, 4, "2025", Modalidade.Fundamental, 2025,
                DateTime.Now, DateTime.Now.AddDays(1), "Descrição inicial",
                30, 25, 5, 20, 10, 10, 8, 35
            );

            resultado.Descricao = "Nova descrição";
            resultado.TotalAlunos = 50;
            resultado.Ano = "2026";

            Assert.Equal("Nova descrição", resultado.Descricao);
            Assert.Equal(50, resultado.TotalAlunos);
            Assert.Equal("2026", resultado.Ano);
        }
    }
}
