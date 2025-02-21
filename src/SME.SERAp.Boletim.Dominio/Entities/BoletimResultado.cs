using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dominio.Entities
{
    public class BoletimResultado : EntidadeBase
    {
        public long CodDre { get; set; }
        public long CodUe { get; set; }
        public string NomeUe { get; set; }
        public long IdProva { get; set; }
        public string NomeProva { get; set; }
        public int AnoEscolar { get; set; }
        public string Turma { get; set; }
        public long EolAluno { get; set; }
        public string NomeAluno { get; set; }
        public string Componente { get; set; }
        public string StatusProva { get; set; }
        public decimal Proficiencia { get; set; }
        public decimal ErroMedida { get; set; }
        public string Nivel { get; set; }
    }
}