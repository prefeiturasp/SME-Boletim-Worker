using SME.SERAp.Boletim.Dominio.Enums;

namespace SME.SERAp.Boletim.Dominio.Entities
{
    public class LoteProva : EntidadeBase
    {
        public LoteProva()
        {
            DataCriacao = DateTime.Now;
        }

        public LoteProva(string nome, bool tipoTai, bool exibirNoBoletim, DateTime dataCorrecaoFim, 
            DateTime dataInicioLote) : this()
        {
            Nome = nome;
            TipoTai = tipoTai;
            ExibirNoBoletim = exibirNoBoletim;
            DataCorrecaoFim = dataCorrecaoFim;
            DataInicioLote = dataInicioLote;
            StatusConsolidacao = LoteStatusConsolidacao.NaoConsolidado;
        }

        public string Nome { get; set; }
        public bool TipoTai { get; set; }
        public bool ExibirNoBoletim { get; set; }
        public DateTime DataCorrecaoFim { get; set; }
        public DateTime DataInicioLote { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public LoteStatusConsolidacao StatusConsolidacao { get; set; }
    }
}
