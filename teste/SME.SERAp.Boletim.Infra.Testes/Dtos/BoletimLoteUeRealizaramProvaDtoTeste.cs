using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos
{
    public class BoletimLoteUeRealizaramProvaDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var dto = new BoletimLoteUeRealizaramProvaDto
            {
                DreId = 10,
                UeId = 20,
                LoteId = 30,
                AnoEscolar = 5,
                RealizaramProva = 150
            };

            Assert.Equal(10, dto.DreId);
            Assert.Equal(20, dto.UeId);
            Assert.Equal(30, dto.LoteId);
            Assert.Equal(5, dto.AnoEscolar);
            Assert.Equal(150, dto.RealizaramProva);
        }
    }
}
