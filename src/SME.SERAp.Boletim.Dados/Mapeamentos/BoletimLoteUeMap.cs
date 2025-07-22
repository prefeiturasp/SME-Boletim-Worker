using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dados.Mapeamentos
{
    public class BoletimLoteUeMap : DommelEntityMap<BoletimLoteUe>
    {
        public BoletimLoteUeMap()
        {
            ToTable("boletim_lote_ue");

            Map(c => c.Id).ToColumn("id").IsKey();


            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.LoteId).ToColumn("lote_id");
            Map(c => c.AnoEscolar).ToColumn("ano_escolar");
            Map(c => c.TotalAlunos).ToColumn("total_alunos");
            Map(c => c.RealizaramProva).ToColumn("realizaram_prova");
        }
    }
}
