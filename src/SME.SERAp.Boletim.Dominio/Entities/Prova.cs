using SME.SERAp.Boletim.Dominio.Enums;

namespace SME.SERAp.Boletim.Dominio.Entities
{
    public class Prova : EntidadeBase
    {
        public string Descricao { get; set; }
        public DateTime? InicioDownload { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public DateTime Inclusao { get; set; }
        public int TotalItens { get; set; }
        public int TempoExecucao { get; set; }
        public long? DisciplinaId { get; set; }
        public string Disciplina { get; set; }
        public long LegadoId { get; set; }
        public string Senha { get; set; }
        public bool PossuiBIB { get; set; }
        public int TotalCadernos { get; set; }
        public Modalidade Modalidade { get; set; }
        public bool OcultarProva { get; set; }
        public bool AderirTodos { get; set; }
        public bool Multidisciplinar { get; set; }
        public int TipoProvaId { get; set; }
        public bool FormatoTai { get; set; }
        public ProvaFormatoTaiItem? ProvaFormatoTaiItem { get; set; }
        public int? QtdItensSincronizacaoRespostas { get; set; }
        public bool PermiteAvancarSemResponderTai { get; set; }
        public bool PermiteVoltarItemAnteriorTai { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public bool ProvaComProficiencia { get; set; }
        public bool ApresentarResultados { get; set; }
        public bool ApresentarResultadosPorItem { get; set; }
        public bool ExibirVideo { get; set; }
        public bool ExibirAudio { get; set; }
        public bool ExibirNoBoletim { get; set; }
    }
}