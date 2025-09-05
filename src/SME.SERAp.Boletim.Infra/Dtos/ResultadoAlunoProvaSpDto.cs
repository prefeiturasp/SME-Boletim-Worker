namespace SME.SERAp.Boletim.Infra.Dtos
{
    public class ResultadoAlunoProvaSpDto
    {
        public string Edicao { get; set; }

        public int AreaConhecimentoID { get; set; }

        public string AnoEscolar { get; set; }

        public string AlunoMatricula { get; set; }

        public int NivelProficiencia { get; set; }

        public decimal Valor { get; set; }
    }
}
