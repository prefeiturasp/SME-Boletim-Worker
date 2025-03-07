namespace SME.SERAp.Boletim.Dominio.Entities
{
    public class BoletimEscolar : EntidadeBase
    {
        public BoletimEscolar()
        {
            
        }

        public BoletimEscolar(long ueId, long provaId, string componenteCurricular, decimal abaixoBasico, decimal abaixoBasicoPorcentagem,
            decimal basico, decimal basicoPorcentagem, decimal adequado, decimal adequadoPorcentagem, decimal avancado, decimal avancadoPorcentagem,
            int total, decimal mediaProficiencia) : this()
        {
            UeId = ueId;
            ProvaId = provaId;
            ComponenteCurricular = componenteCurricular;
            AbaixoBasico = abaixoBasico;
            AbaixoBasicoPorcentagem = abaixoBasicoPorcentagem;
            Basico = basico;
            BasicoPorcentagem = basicoPorcentagem;
            Adequado = adequado;
            AdequadoPorcentagem = adequadoPorcentagem;
            Avancado = avancado;
            AvancadoPorcentagem = avancadoPorcentagem;
            Total = total;
            MediaProficiencia = mediaProficiencia;
        }

        public long UeId { get;set; }

        public long ProvaId { get;set; }

        public string ComponenteCurricular { get;set; }

        public decimal AbaixoBasico { get; set; }

        public decimal AbaixoBasicoPorcentagem { get; set; }

        public decimal Basico { get; set; }

        public decimal BasicoPorcentagem { get; set; }

        public decimal Adequado { get; set; }

        public decimal AdequadoPorcentagem { get; set; }

        public decimal Avancado { get; set; }

        public decimal AvancadoPorcentagem { get; set; }

        public int Total { get; set; }

        public decimal MediaProficiencia { get; set; }
    }
}
