namespace SME.SERAp.Boletim.Dominio.Entities
{
    public class NivelProficiencia : EntidadeBase
    {
        public int Codigo { get;set; }

        public string Descricao { get; set; }

        public long? ValorReferencia { get; set; }

        public long DisciplinaId { get; set; }

        public long Ano { get;set; }
    }
}
