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

        public decimal Proficiencia { get; set; }

        public IEnumerable<AlunoQuestaoBoletimResultadoProbabilidadeDto> Questoes { get; set; }

        public void AdicionarQuestao(string codigoHabilidade, string descricaoHabilidade,
            long questaoLegadoId, decimal probabilidade)
        {
            Questoes ??= new List<AlunoQuestaoBoletimResultadoProbabilidadeDto>();
            Questoes.Append(new AlunoQuestaoBoletimResultadoProbabilidadeDto(codigoHabilidade, descricaoHabilidade, 
                questaoLegadoId, probabilidade));
        }
    }
}
