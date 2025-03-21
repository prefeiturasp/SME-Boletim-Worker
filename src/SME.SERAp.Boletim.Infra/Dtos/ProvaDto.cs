using SME.SERAp.Boletim.Dominio.Enums;

namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class ProvaDto
    {
        public long Id { get; set; }
        public long Codigo { get; set; }
        public string Descricao { get; set; }
        public Modalidade Modalidade { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public DateTime DataCorrecaoInicio { get; set; }
        public DateTime DataCorrecaoFim { get; set; }
        public int QuantidadeQuestoes { get; set; }
        public bool ExibirNoBoletim { get; set; }
        public long LoteId { get; set; }
        public bool FormatoTai { get; set; }
    }
}