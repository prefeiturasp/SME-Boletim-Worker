namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class TurmaBoletimResultadoProbabilidadeDto
    {
        public long ProvaId { get; set; }

        public long UeId { get; set; }

        public long TurmaId { get; set; }

        public string TurmaNome { get; set; }

        public int AnoLetivo { get; set; }

        public int DisciplinaId { get; set; }

        public int AnoEscolar { get; set; }
    }
}
