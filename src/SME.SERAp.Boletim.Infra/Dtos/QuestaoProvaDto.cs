namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class QuestaoProvaDto
    {
        public string DescricaoHabilidade { get; set; }

        public string CodigoHabilidade { get; set; }

        public long HabilidadeId { get; set; }  

        public long QuestaoLegadoId { get; set; }

        public decimal Discriminacao { get; set; }

        public decimal Dificuldade { get; set; }

        public decimal AcertoCasual { get; set; }

        public decimal Probabilidade { get; set; }
    }
}
