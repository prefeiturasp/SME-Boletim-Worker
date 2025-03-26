namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class AlunoQuestaoBoletimResultadoProbabilidadeDto
    {
        public AlunoQuestaoBoletimResultadoProbabilidadeDto(string codigoHabilidade, string descricaoHabilidade, long questaoLegadoId, double probabilidade)
        {
            CodigoHabilidade = codigoHabilidade;
            DescricaoHabilidade = descricaoHabilidade;
            QuestaoLegadoId = questaoLegadoId;
            Probabilidade = probabilidade;
        }

        public string CodigoHabilidade { get; set; }

        public string DescricaoHabilidade { get; set; }

        public long QuestaoLegadoId { get; set; }

        public double Probabilidade { get; set; }

        public int NivelProficiencia { get; set; }
    }
}
