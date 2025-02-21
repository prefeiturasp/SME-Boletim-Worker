using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.SerapEstudantes
{
    public class SituacaoTurmaProvaDto
    {
        public long TotalIniciadoHoje { get; set; }
        public long TotalIniciadoNaoFinalizado { get; set; }
        public long TotalFinalizado { get; set; }
    }
}