using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using SME.SERAp.Boletim.Dados.Mapeamentos;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dados.Testes.Mapeamentos
{
    public class BoletimLoteUeMapTeste
    {
        static BoletimLoteUeMapTeste()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new BoletimLoteUeMap());
                config.ForDommel();
            });
        }

        [Fact]
        public void Deve_Mapear_Entidade_Para_Tabela_Correta()
        {
            var map = (BoletimLoteUeMap)FluentMapper.EntityMaps[typeof(BoletimLoteUe)];
            Assert.Equal("boletim_lote_ue", map.TableName);
        }

        [Fact]
        public void Deve_Mapear_Propriedades_Para_Colunas_Corretas()
        {
            var map = (BoletimLoteUeMap)FluentMapper.EntityMaps[typeof(BoletimLoteUe)];

            Assert.Equal("id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimLoteUe.Id)).ColumnName);
            Assert.Equal("dre_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimLoteUe.DreId)).ColumnName);
            Assert.Equal("ue_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimLoteUe.UeId)).ColumnName);
            Assert.Equal("lote_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimLoteUe.LoteId)).ColumnName);
            Assert.Equal("ano_escolar", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimLoteUe.AnoEscolar)).ColumnName);
            Assert.Equal("total_alunos", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimLoteUe.TotalAlunos)).ColumnName);
            Assert.Equal("realizaram_prova", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimLoteUe.RealizaramProva)).ColumnName);
        }

        [Fact]
        public void Deve_Definir_Id_Como_Chave_Primaria()
        {
            var map = (BoletimLoteUeMap)FluentMapper.EntityMaps[typeof(BoletimLoteUe)];
            var idColumn = map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(BoletimLoteUe.Id)).ColumnName;
            Assert.Equal("id", idColumn);
        }
    }
}
