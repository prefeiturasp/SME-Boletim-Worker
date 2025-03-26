namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class AlunoBoletimResultadoProbabilidadeDto
    {
        public long Id { get; set; }

        public string Nome { get; set; }

        public long Codigo { get; set; }

        public long TurmaId { get; set; }

        public string TurmaNome { get; set; }

        public long UeId { get; set; }

        public double Proficiencia { get; set; }

        public int NivelProficiencia { get; set; }

        public List<AlunoQuestaoBoletimResultadoProbabilidadeDto> Questoes { get; set; }

        public void AdicionarQuestao(string codigoHabilidade, string descricaoHabilidade,
            long questaoLegadoId, double probabilidade)
        {
            Questoes ??= new List<AlunoQuestaoBoletimResultadoProbabilidadeDto>();
            Questoes.Add(new AlunoQuestaoBoletimResultadoProbabilidadeDto(codigoHabilidade, descricaoHabilidade, 
                questaoLegadoId, probabilidade));
        }
    }
}
