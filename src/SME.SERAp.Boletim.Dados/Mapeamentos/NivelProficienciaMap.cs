using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dados.Mapeamentos
{
    public class NivelProficienciaMap : DommelEntityMap<NivelProficiencia>
    {
        public NivelProficienciaMap()
        {
            ToTable("nivel_proficiencia");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.Codigo).ToColumn("codigo");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.ValorReferencia).ToColumn("valor_referencia");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.Ano).ToColumn("ano");
        }
    }
}
