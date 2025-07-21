namespace SME.SERAp.Boletim.Dominio.Entities
{
    public class BoletimLoteUe : EntidadeBase
    {
        public long DreId { get; set; }

        public long UeId { get; set; }

        public long LoteId { get; set; }

        public int AnoEscolar { get; set; }

        public int TotalAlunos { get; set; }

        public int RealizaramProva { get; set; }
    }
}
