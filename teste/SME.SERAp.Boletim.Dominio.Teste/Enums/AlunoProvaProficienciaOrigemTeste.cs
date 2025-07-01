using SME.SERAp.Boletim.Dominio.Enums;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Enums
{
    public class AlunoProvaProficienciaOrigemTeste
    {
        [Fact]
        public void ValoresDoEnumDevemEstarCorretos()
        {
            Assert.Equal(0, (int)AlunoProvaProficienciaOrigem.TAI_estudante);
            Assert.Equal(1, (int)AlunoProvaProficienciaOrigem.PSP_estudante);
            Assert.Equal(2, (int)AlunoProvaProficienciaOrigem.PSP_ano_escolar);
            Assert.Equal(3, (int)AlunoProvaProficienciaOrigem.PSP_Dre);
        }

        [Fact]
        public void Enum_Parse_DeveFuncionarCorretamente()
        {
            var valor = (AlunoProvaProficienciaOrigem)System.Enum.Parse(typeof(AlunoProvaProficienciaOrigem), "PSP_estudante");
            Assert.Equal(AlunoProvaProficienciaOrigem.PSP_estudante, valor);
        }
    }
}
