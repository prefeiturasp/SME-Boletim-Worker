using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using SME.SERAp.Boletim.Dados.Mapeamentos;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dados.Testes.Mapeamentos
{
    public class NivelProficienciaMapTeste
    {
        static NivelProficienciaMapTeste()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new NivelProficienciaMap());
                config.ForDommel();
            });
        }

        [Fact]
        public void Deve_Mapear_Entidade_Para_Tabela_Correta()
        {
            var map = (NivelProficienciaMap)FluentMapper.EntityMaps[typeof(NivelProficiencia)];
            Assert.Equal("nivel_proficiencia", map.TableName);
        }

        [Fact]
        public void Deve_Mapear_Propriedades_Para_Colunas_Corretas()
        {
            var map = (NivelProficienciaMap)FluentMapper.EntityMaps[typeof(NivelProficiencia)];

            Assert.Equal("id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(NivelProficiencia.Id)).ColumnName);
            Assert.Equal("codigo", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(NivelProficiencia.Codigo)).ColumnName);
            Assert.Equal("descricao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(NivelProficiencia.Descricao)).ColumnName);
            Assert.Equal("valor_referencia", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(NivelProficiencia.ValorReferencia)).ColumnName);
            Assert.Equal("disciplina_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(NivelProficiencia.DisciplinaId)).ColumnName);
            Assert.Equal("ano", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(NivelProficiencia.Ano)).ColumnName);
        }

        [Fact]
        public void Deve_Definir_Id_Como_Chave_Primaria()
        {
            var map = (NivelProficienciaMap)FluentMapper.EntityMaps[typeof(NivelProficiencia)];
            var idColumn = map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(NivelProficiencia.Id)).ColumnName;
            Assert.Equal("id", idColumn);
        }
    }
}
