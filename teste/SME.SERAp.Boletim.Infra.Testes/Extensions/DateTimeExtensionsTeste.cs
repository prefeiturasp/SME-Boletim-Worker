using SME.SERAp.Boletim.Infra.Extensions;

namespace SME.SERAp.Boletim.Infra.Testes.Extensions
{
    public class DateTimeExtensionsTeste
    {
        [Fact]
        public void Deve_Retornar_InicioDoMes_Corretamente()
        {
            var data = new DateTime(2025, 8, 22, 15, 30, 45);
            var inicioMes = data.InicioMes();

            Assert.Equal(new DateTime(2025, 8, 1, 0, 0, 0), inicioMes);
        }

        [Fact]
        public void Deve_Retornar_FinalDoMes_Corretamente()
        {
            var data = new DateTime(2025, 2, 15, 10, 20, 30);
            var finalMes = data.FinalMes();

            Assert.Equal(new DateTime(2025, 2, 28, 23, 59, 59), finalMes);
        }

        [Fact]
        public void Deve_Retornar_FinalDoMes_Corretamente_Em_AnoBissexto()
        {
            var data = new DateTime(2024, 2, 10);
            var finalMes = data.FinalMes();

            Assert.Equal(new DateTime(2024, 2, 29, 23, 59, 59), finalMes);
        }
    }
}
