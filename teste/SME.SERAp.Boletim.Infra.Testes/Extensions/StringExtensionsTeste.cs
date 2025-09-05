using SME.SERAp.Boletim.Infra.Extensions;

namespace SME.SERAp.Boletim.Infra.Testes.Extensions
{
    public class StringExtensionsTeste
    {
        [Theory]
        [InlineData("123", 123)]
        [InlineData("0", 0)]
        [InlineData("-456", -456)]
        public void Deve_Retornar_Numero_Correto_Quando_String_Valida(string entrada, int esperado)
        {
            var resultado = entrada.ConverterParaInt();

            Assert.Equal(esperado, resultado);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("12.34")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void Deve_Retornar_Zero_Quando_String_Invalida(string entrada)
        {
            var resultado = entrada.ConverterParaInt();

            Assert.Equal(0, resultado);
        }
    }
}
