using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dados.Mapeamentos
{
    public class BoletimLoteProvaMap : DommelEntityMap<BoletimLoteProva>
    {
        public BoletimLoteProvaMap()
        {
            ToTable("boletim_lote_prova");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.LoteId).ToColumn("lote_id");
            Map(c => c.ProvaId).ToColumn("prova_id");
            Map(c => c.ExibirNoBoletim).ToColumn("exibir_no_boletim");
        }
    }
}
