namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class BoletimResultadoProbabilidadeDto
    {
        public BoletimResultadoProbabilidadeDto()
        {
            
        }

        public BoletimResultadoProbabilidadeDto(long habilidadeId, string codigoHabilidade, string habilidadeDescricao, string turmaDescricao,
            long turmaId, long provaId, long ueId, long disciplinaId, long anoEscolar, decimal abaixoDoBasico, decimal basico, decimal adequado, decimal avancado) : this()
        {
            HabilidadeId = habilidadeId;
            CodigoHabilidade = codigoHabilidade;
            HabilidadeDescricao = habilidadeDescricao;
            TurmaDescricao = turmaDescricao;
            TurmaId = turmaId;
            ProvaId = provaId;
            UeId = ueId;
            DisciplinaId = disciplinaId;
            AnoEscolar = anoEscolar;
            AbaixoDoBasico = abaixoDoBasico;
            Basico = basico;
            Adequado = adequado;
            Avancado = avancado;
        }

        public long HabilidadeId { get; set; }

        public string CodigoHabilidade { get; set; }

        public string HabilidadeDescricao { get; set; }

        public string TurmaDescricao { get; set; }

        public long TurmaId { get; set; }

        public long ProvaId { get; set; }

        public long UeId { get; set; }

        public long DisciplinaId { get; set; }

        public long AnoEscolar { get; set; }

        public decimal AbaixoDoBasico { get; set; }

        public decimal Basico { get; set; }

        public decimal Adequado { get; set; }

        public decimal Avancado { get; set; }
    }
}
