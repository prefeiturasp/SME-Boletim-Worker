using SME.SERAp.Boletim.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos.SerapEstudantes
{
    public class ProvaDto
    {
        public long Id { get; set; }
        public long Codigo { get; set; }
        public string Descricao { get; set; }
        public Modalidade Modalidade { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public int QuantidadeQuestoes { get; set; }
    }
}