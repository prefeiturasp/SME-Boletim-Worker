namespace SME.SERAp.Boletim.Dominio.Entities
{
    public class BoletimResultadoProbabilidade : EntidadeBase
    {
        public BoletimResultadoProbabilidade()
        {
            
        }

        public BoletimResultadoProbabilidade(long habilidadeId, string codigoHabilidade, string habilidadeDescricao, string turmaDescricao,
            long turmaId, long provaId, long ueId,  long disciplinaId, long anoEscolar, double abaixoDoBasico, double basico, double adequado, double avancado) : this()
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
            DataConsolidacao = DateTime.Now;
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

        public double AbaixoDoBasico { get; set; }

        public double Basico { get; set; }

        public double Adequado { get; set; }

        public double Avancado { get; set; }

        public DateTime DataConsolidacao { get; set; }
    }
}
