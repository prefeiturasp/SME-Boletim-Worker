﻿namespace SME.SERAp.Boletim.Dominio.Entities
{
    public class AlunoProvaSpProficiencia : EntidadeBase
    {
        public long AlunoRa { get; set; }

        public int AnoEscolar { get; set; }

        public int AnoLetivo { get; set; }

        public long DisciplinaId { get; set; }

        public decimal Proficiencia { get; set; }
        public int NivelProficiencia { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
