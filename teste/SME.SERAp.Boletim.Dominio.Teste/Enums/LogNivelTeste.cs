using SME.SERAp.Boletim.Dominio.Enums;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Enums
{
    public class LogNivelTeste
    {
        [Fact]
        public void ValoresDoEnumDevemEstarCorretos()
        {
            Assert.Equal(1, (int)LogNivel.Informacao);
            Assert.Equal(2, (int)LogNivel.Critico);
            Assert.Equal(3, (int)LogNivel.Negocio);
        }

        [Fact]
        public void Enum_Parse_DeveFuncionarCorretamente()
        {
            var valor = (LogNivel)System.Enum.Parse(typeof(LogNivel), "Critico");
            Assert.Equal(LogNivel.Critico, valor);
        }
    }
}
