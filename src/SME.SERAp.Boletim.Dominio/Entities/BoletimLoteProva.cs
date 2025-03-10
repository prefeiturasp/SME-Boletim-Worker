namespace SME.SERAp.Boletim.Dominio.Entities
{
    public class BoletimLoteProva : EntidadeBase
    {
        public BoletimLoteProva()
        {
            
        }

        public BoletimLoteProva(long provaId, long loteId, bool exibirNoBoletim) : this()
        {
            ProvaId = provaId;
            LoteId = loteId;
            ExibirNoBoletim = exibirNoBoletim;
        }

        public long ProvaId { get; set; }

        public long LoteId { get; set; }

        public bool ExibirNoBoletim { get; set; }
    }
}
