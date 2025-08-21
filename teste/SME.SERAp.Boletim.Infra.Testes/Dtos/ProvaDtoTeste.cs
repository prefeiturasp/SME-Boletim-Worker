using SME.SERAp.Boletim.Dominio.Enums;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos
{
    public class ProvaDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var inicio = new DateTime(2025, 1, 10);
            var fim = new DateTime(2025, 1, 20);
            var correcaoInicio = new DateTime(2025, 1, 21);
            var correcaoFim = new DateTime(2025, 1, 30);

            var dto = new ProvaDto
            {
                Id = 1,
                Codigo = 12345,
                Descricao = "Prova de Matemática",
                Modalidade = Modalidade.Fundamental,
                Inicio = inicio,
                Fim = fim,
                DataCorrecaoInicio = correcaoInicio,
                DataCorrecaoFim = correcaoFim,
                QuantidadeQuestoes = 20,
                ExibirNoBoletim = true,
                LoteId = 99,
                FormatoTai = false,
                AnoEscolar = 5,
                DisciplinaId = 10
            };

            Assert.Equal(1, dto.Id);
            Assert.Equal(12345, dto.Codigo);
            Assert.Equal("Prova de Matemática", dto.Descricao);
            Assert.Equal(Modalidade.Fundamental, dto.Modalidade);
            Assert.Equal(inicio, dto.Inicio);
            Assert.Equal(fim, dto.Fim);
            Assert.Equal(correcaoInicio, dto.DataCorrecaoInicio);
            Assert.Equal(correcaoFim, dto.DataCorrecaoFim);
            Assert.Equal(20, dto.QuantidadeQuestoes);
            Assert.True(dto.ExibirNoBoletim);
            Assert.Equal(99, dto.LoteId);
            Assert.False(dto.FormatoTai);
            Assert.Equal(5, dto.AnoEscolar);
            Assert.Equal(10, dto.DisciplinaId);
        }
    }
}
