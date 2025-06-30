using SME.SERAp.Boletim.Dominio.Enums;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Enums
{
    public class AlunoProvaProficienciaTipoTeste
    {
        [Fact]
        public void ValoresDoEnumDevemEstarCorretos()
        {
            Assert.Equal(0, (int)AlunoProvaProficienciaTipo.Inicial);
            Assert.Equal(1, (int)AlunoProvaProficienciaTipo.Parcial);
            Assert.Equal(2, (int)AlunoProvaProficienciaTipo.Final);
        }

        [Fact]
        public void Enum_Parse_DeveFuncionarCorretamente()
        {
            var valor = (AlunoProvaProficienciaTipo)System.Enum.Parse(typeof(AlunoProvaProficienciaTipo), "Parcial");
            Assert.Equal(AlunoProvaProficienciaTipo.Parcial, valor);
        }
    }
}
