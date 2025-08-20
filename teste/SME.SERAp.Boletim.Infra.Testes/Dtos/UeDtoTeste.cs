using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos
{
    public class UeDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var dto = new UeDto
            {
                Id = 1,
                DreId = 10,
                Nome = "Escola Modelo"
            };

            Assert.Equal(1, dto.Id);
            Assert.Equal(10, dto.DreId);
            Assert.Equal("Escola Modelo", dto.Nome);
        }
    }
}
