using SME.SERAp.Boletim.Dominio.Enums;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos
{
    public class ProvaTurmaDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var inicio = new DateTime(2025, 2, 10);
            var fim = new DateTime(2025, 2, 20);

            var dto = new ProvaTurmaDto
            {
                AnoLetivo = 2025,
                ProvaId = 123,
                Inicio = inicio,
                Fim = fim,
                DreId = 10,
                UeId = 20,
                Ano = "5º Ano",
                Modalidade = Modalidade.Fundamental,
                TurmaId = 999,
                Descricao = "Prova de Ciências",
                QuantidadeQuestoes = 15,
                AderirTodos = true,
                Deficiente = false
            };

            Assert.Equal(2025, dto.AnoLetivo);
            Assert.Equal(123, dto.ProvaId);
            Assert.Equal(inicio, dto.Inicio);
            Assert.Equal(fim, dto.Fim);
            Assert.Equal(10, dto.DreId);
            Assert.Equal(20, dto.UeId);
            Assert.Equal("5º Ano", dto.Ano);
            Assert.Equal(Modalidade.Fundamental, dto.Modalidade);
            Assert.Equal(999, dto.TurmaId);
            Assert.Equal("Prova de Ciências", dto.Descricao);
            Assert.Equal(15, dto.QuantidadeQuestoes);
            Assert.True(dto.AderirTodos);
            Assert.False(dto.Deficiente);
        }
    }
}
