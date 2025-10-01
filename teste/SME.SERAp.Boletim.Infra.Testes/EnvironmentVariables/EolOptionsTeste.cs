using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Infra.Testes.EnvironmentVariables
{
    public class EolOptionsTeste
    {
        [Fact]
        public void Deve_Retornar_Valor_Secao_Esperado()
        {
            var secao = EolOptions.Secao;

            Assert.Equal("Eol", secao);
        }

        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var options = new EolOptions
            {
                TiposEscola = "Municipal"
            };

            Assert.Equal("Municipal", options.TiposEscola);
        }
    }
}
