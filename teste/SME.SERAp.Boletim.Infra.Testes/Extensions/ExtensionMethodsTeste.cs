using SME.SERAp.Boletim.Infra.Extensions;

namespace SME.SERAp.Boletim.Infra.Testes.Extensions
{
    public class ExtensionMethodsTeste
    {
        private class ClasseTeste
        {
            public const int ConstanteInt = 42;
            public const string ConstanteString = "Teste";
            public static string OutraConstanteString = "Teste 2";

            public string MetodoSimples() => "Ok";

            public async Task<string> MetodoAsync()
            {
                await Task.Delay(10);
                return "AsyncOk";
            }
        }

        [Fact]
        public void Deve_Obter_ConstantesPublicas_Corretamente()
        {
            var constantesInt = typeof(ClasseTeste).ObterConstantesPublicas<int>();
            var constantesString = typeof(ClasseTeste).ObterConstantesPublicas<string>();

            Assert.Single(constantesInt);
            Assert.Equal(42, constantesInt.First());

            Assert.Single(constantesString);
            Assert.Equal("Teste", constantesString.First());
        }

        [Fact]
        public void Deve_Obter_Metodo_Corretamente()
        {
            var metodo = typeof(ClasseTeste).ObterMetodo("MetodoSimples");

            Assert.NotNull(metodo);
            Assert.Equal("MetodoSimples", metodo.Name);
        }

        [Fact]
        public async Task Deve_InvokeAsync_Corretamente()
        {
            var instancia = new ClasseTeste();
            var metodo = typeof(ClasseTeste).ObterMetodo("MetodoAsync");

            var resultado = await metodo.InvokeAsync(instancia);

            Assert.Equal("AsyncOk", resultado);
        }
    }
}
