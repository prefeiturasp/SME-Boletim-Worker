using SME.SERAp.Boletim.Dominio.Enums;

namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class AlunoProvaProficienciaBoletimDto
    {
        public long CodigoDre { get; set; }

        public string CodigoUe { get; set; }

        public string NomeUe { get; set; }

        public long ProvaId { get; set; }

        public string NomeProva { get; set; }

        public int AnoEscolar { get; set; }

        public string Turma { get; set; }

        public long CodigoAluno { get; set; }

        public string NomeAluno { get; set; }

        public string NomeDisciplina { get; set; }

        public long DisciplinaId { get; set; }

        public ProvaStatus ProvaStatus { get; set; }

        public decimal Profeciencia { get; set; }

        public decimal ErroMedida { get; set; }

        public long NivelCodigo { get; set; }

        public long BoletimLoteId { get; set; }
    }
}
