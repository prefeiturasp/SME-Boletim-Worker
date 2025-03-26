namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class QuestaoProvaDto
    {
        public string DescricaoHabilidade { get; set; }

        public string CodigoHabilidade { get; set; }

        public long HabilidadeId { get; set; }  

        public long QuestaoLegadoId { get; set; }

        public double Discriminacao { get; set; }

        public double Dificuldade { get; set; }

        public double AcertoCasual { get; set; }

        public Dictionary<int, double> Probabilidades { get; set; }
    }
}
