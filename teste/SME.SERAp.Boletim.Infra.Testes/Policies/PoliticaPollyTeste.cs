using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Policies;

namespace SME.SERAp.Boletim.Infra.Testes.Policies
{
    public class PoliticaPollyTeste
    {
        [Fact]
        public void Deve_Retornar_Valores_Das_Constantes_Corretamente()
        {
            Assert.Equal("RetryPolicyFilasRabbit", PoliticaPolly.PublicaFila);
        }
    }
}
