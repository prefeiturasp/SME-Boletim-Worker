using SME.SERAp.Boletim.Dominio.Enums;

namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class ProvaTurmaDto
    {
        public int AnoLetivo { get; set; }
        public long ProvaId { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public string Ano { get; set; }
        public Modalidade Modalidade { get; set; }
        public long TurmaId { get; set; }
        public string Descricao { get; set; }
        public int QuantidadeQuestoes { get; set; }
        public bool AderirTodos { get; set; }
        public bool Deficiente { get; set; }
    }
}