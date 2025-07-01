using SME.SERAp.Boletim.Dominio.Enums;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Enums
{
    public class LoteStatusConsolidacaoTeste
    {
        [Fact]
        public void ValoresDoEnumDevemEstarCorretos()
        {
            Assert.Equal(0, (int)LoteStatusConsolidacao.NaoConsolidado);
            Assert.Equal(1, (int)LoteStatusConsolidacao.Pendente);
            Assert.Equal(2, (int)LoteStatusConsolidacao.Consolidado);
        }

        [Fact]
        public void Enum_Parse_DeveFuncionarCorretamente()
        {
            var valor = (LoteStatusConsolidacao)System.Enum.Parse(typeof(LoteStatusConsolidacao), "Pendente");
            Assert.Equal(LoteStatusConsolidacao.Pendente, valor);
        }
    }
}
