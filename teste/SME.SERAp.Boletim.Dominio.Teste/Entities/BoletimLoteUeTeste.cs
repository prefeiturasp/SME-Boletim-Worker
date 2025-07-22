using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dominio.Teste.Entities
{
    public class BoletimLoteUeTeste
    {
        [Fact]
        public void ConstrutorPadrao_DeveInicializarComValoresPadrao()
        {
            var dreId = 1L;
            var ueId = 2L;
            var loteId = 3L;
            var anoEscolar = 4;
            var totalAlunos = 5;
            var realizaramProva = 6;

            var boletimLoteUe = new BoletimLoteUe(dreId, ueId, loteId, anoEscolar, totalAlunos, realizaramProva);

            Assert.Equal(dreId, boletimLoteUe.DreId);
            Assert.Equal(ueId, boletimLoteUe.UeId);
            Assert.Equal(loteId, boletimLoteUe.LoteId);
            Assert.Equal(anoEscolar, boletimLoteUe.AnoEscolar);
            Assert.Equal(totalAlunos, boletimLoteUe.TotalAlunos);
            Assert.Equal(realizaramProva, boletimLoteUe.RealizaramProva);
        }
    }
}
