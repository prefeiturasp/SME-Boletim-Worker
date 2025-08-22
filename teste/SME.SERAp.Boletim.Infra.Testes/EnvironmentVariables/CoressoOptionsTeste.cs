using SME.SERAp.Boletim.Infra.EnvironmentVariables;

namespace SME.SERAp.Boletim.Infra.Testes.EnvironmentVariables
{
    public class CoressoOptionsTeste
    {
        [Fact]
        public void Deve_Retornar_Valor_Secao_Esperado()
        {
            var secao = CoressoOptions.Secao;

            Assert.Equal("Coresso", secao);
        }

        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var options = new CoressoOptions
            {
                SistemaId = 123,
                AcompanhamentoModuloId = 456
            };

            Assert.Equal(123, options.SistemaId);
            Assert.Equal(456, options.AcompanhamentoModuloId);
        }
    }
}
