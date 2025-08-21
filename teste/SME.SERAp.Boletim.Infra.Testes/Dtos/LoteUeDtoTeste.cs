using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos
{
    public class LoteUeDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var dto = new LoteUeDto
            {
                LoteId = 100,
                ProvasIds = new List<long> { 1, 2, 3 },
                UeId = 200,
                DreId = 300,
                AnoEscolar = 5
            };

            Assert.Equal(100, dto.LoteId);
            Assert.Equal(new List<long> { 1, 2, 3 }, dto.ProvasIds);
            Assert.Equal(200, dto.UeId);
            Assert.Equal(300, dto.DreId);
            Assert.Equal(5, dto.AnoEscolar);
        }
    }
}
