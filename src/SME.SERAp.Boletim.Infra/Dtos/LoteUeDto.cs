namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class LoteUeDto
    {
        public long LoteId { get; set; }

        public IEnumerable<long> ProvasIds { get; set; }

        public long UeId { get; set; }

        public long DreId { get; set; }

        public int AnoEscolar { get; set; }
    }
}
