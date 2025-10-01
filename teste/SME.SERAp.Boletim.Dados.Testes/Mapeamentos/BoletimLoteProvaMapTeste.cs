using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using SME.SERAp.Boletim.Dados.Mapeamentos;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dados.Testes.Mapeamentos
{
    public class BoletimLoteProvaMapTeste
    {
        static BoletimLoteProvaMapTeste()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new BoletimLoteProvaMap());
                config.ForDommel();
            });
        }

        [Fact]
        public void Deve_Mapear_Entidade_Para_Tabela_Correta()
        {
            var map = (BoletimLoteProvaMap)FluentMapper.EntityMaps[typeof(BoletimLoteProva)];
            Assert.Equal("boletim_lote_prova", map.TableName);
        }

        [Fact]
        public void Deve_Mapear_Propriedades_Para_Colunas_Corretas()
        {
            var map = (BoletimLoteProvaMap)FluentMapper.EntityMaps[typeof(BoletimLoteProva)];

            Assert.Equal("id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimLoteProva.Id)).ColumnName);
            Assert.Equal("lote_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimLoteProva.LoteId)).ColumnName);
            Assert.Equal("prova_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimLoteProva.ProvaId)).ColumnName);
        }

        [Fact]
        public void Deve_Definir_Id_Como_Chave_Primaria()
        {
            var map = (BoletimLoteProvaMap)FluentMapper.EntityMaps[typeof(BoletimLoteProva)];
            var idColumn = map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimLoteProva.Id)).ColumnName;
            Assert.Equal("id", idColumn);
        }
    }
}
