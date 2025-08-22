using SME.SERAp.Boletim.Infra.Extensions;

namespace SME.SERAp.Boletim.Infra.Testes.Extensions
{
    public class SerializerExtensionsTeste
    {
        private class ClasseTeste
        {
            public int Id { get; set; }
            public string Nome { get; set; }
        }

        [Fact]
        public void Deve_Converter_Corretamente_Utilizando_ConverterObjectStringPraObjeto()
        {
            var json = "{\"Id\":1,\"Nome\":\"Teste\"}";

            var objeto = json.ConverterObjectStringPraObjeto<ClasseTeste>();

            Assert.NotNull(objeto);
            Assert.Equal(1, objeto.Id);
            Assert.Equal("Teste", objeto.Nome);
        }

        [Fact]
        public void Deve_Retornar_Default_Se_String_Nula_Ou_Vazia_Utilizando_ConverterObjectStringPraObjeto()
        {
            string jsonNulo = null;
            string jsonVazio = string.Empty;

            var objetoNulo = jsonNulo.ConverterObjectStringPraObjeto<ClasseTeste>();
            var objetoVazio = jsonVazio.ConverterObjectStringPraObjeto<ClasseTeste>();

            Assert.Null(objetoNulo);
            Assert.Null(objetoVazio);
        }

        [Fact]
        public void Deve_Converter_Corretamente_Utilizando_ConverterObjectParaJson()
        {
            var objeto = new ClasseTeste { Id = 2, Nome = "OutroTeste" };

            var json = objeto.ConverterObjectParaJson();

            Assert.Contains("\"Id\":2", json);
            Assert.Contains("\"Nome\":\"OutroTeste\"", json);
        }

        [Fact]
        public void Deve_Retornar_StringVazia_Se_Nulo_Utilizando_ConverterObjectParaJson()
        {
            ClasseTeste objeto = null;

            var json = objeto.ConverterObjectParaJson();

            Assert.Equal(string.Empty, json);
        }
    }
}
