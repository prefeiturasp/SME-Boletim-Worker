using SME.SERAp.Boletim.Dominio.Entities;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Entities
{
    public class BoletimLoteProvaTeste
    {
        [Fact]
        public void Construtor_DeveAtribuirProvaIdELoteIdCorretamente()
        {
            long provaId = 123;
            long loteId = 456;

            var boletim = new BoletimLoteProva(provaId, loteId);

            Assert.Equal(provaId, boletim.ProvaId);
            Assert.Equal(loteId, boletim.LoteId);
        }

        [Fact]
        public void ConstrutorPadrao_DeveInicializarComValoresPadrao()
        {
            var boletim = new BoletimLoteProva();

            Assert.Equal(0, boletim.ProvaId);
            Assert.Equal(0, boletim.LoteId);
        }
    }
}
