using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.SerapEstudantes
{
    public class SituacaoAlunoProvaDto
    {
        public bool FezDownload { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Fim { get; set; }
        public int? Tempo { get; set; }
        public int? QuestaoRespondida { get; set; }
        public string UsuarioIdReabertura { get; set; }
        public DateTime? DataHoraReabertura { get; set; }
    }
}