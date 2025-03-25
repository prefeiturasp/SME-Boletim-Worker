namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class AlunoQuestaoBoletimResultadoProbabilidadeDto
    {
        public AlunoQuestaoBoletimResultadoProbabilidadeDto(string codigoHabilidade, string descricaoHabilidade, long questaoLegadoId, decimal probabilidade)
        {
            CodigoHabilidade = codigoHabilidade;
            DescricaoHabilidade = descricaoHabilidade;
            QuestaoLegadoId = questaoLegadoId;
            Probabilidade = probabilidade;
        }

        public string CodigoHabilidade { get; set; }

        public string DescricaoHabilidade { get; set; }

        public long QuestaoLegadoId { get; set; }

        public decimal Probabilidade { get; set; }
    }
}
