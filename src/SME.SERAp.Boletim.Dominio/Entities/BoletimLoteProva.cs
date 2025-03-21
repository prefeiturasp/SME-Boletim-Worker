namespace SME.SERAp.Boletim.Dominio.Entities
{
    public class BoletimLoteProva : EntidadeBase
    {
        public BoletimLoteProva()
        {
            
        }

        public BoletimLoteProva(long provaId, long loteId) : this()
        {
            ProvaId = provaId;
            LoteId = loteId;
        }

        public long ProvaId { get; set; }

        public long LoteId { get; set; }
    }
}
