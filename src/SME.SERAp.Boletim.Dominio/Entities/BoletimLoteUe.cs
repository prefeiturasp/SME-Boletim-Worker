namespace SME.SERAp.Boletim.Dominio.Entities
{
    public class BoletimLoteUe : EntidadeBase
    {
        public BoletimLoteUe()
        {
            
        }

        public BoletimLoteUe(long dreId, long ueId, long loteId, int anoEscolar, int totalAlunos, int realizaramProva)
        {
            DreId = dreId;
            UeId = ueId;
            LoteId = loteId;
            AnoEscolar = anoEscolar;
            TotalAlunos = totalAlunos;
            RealizaramProva = realizaramProva;
        }

        public long DreId { get; set; }

        public long UeId { get; set; }

        public long LoteId { get; set; }

        public int AnoEscolar { get; set; }

        public int TotalAlunos { get; set; }

        public int RealizaramProva { get; set; }
    }
}
