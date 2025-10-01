using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using SME.SERAp.Boletim.Dados.Mapeamentos;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dados.Testes.Mapeamentos
{
    public class ProvaMapTeste
    {
        static ProvaMapTeste()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new ProvaMap());
                config.ForDommel();
            });
        }

        [Fact]
        public void Deve_Mapear_Entidade_Para_Tabela_Correta()
        {
            var map = (ProvaMap)FluentMapper.EntityMaps[typeof(Prova)];
            Assert.Equal("prova", map.TableName);
        }

        [Fact]
        public void Deve_Mapear_Propriedades_Para_Colunas_Corretas()
        {
            var map = (ProvaMap)FluentMapper.EntityMaps[typeof(Prova)];

            Assert.Equal("id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.Id)).ColumnName);
            Assert.Equal("prova_legado_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.LegadoId)).ColumnName);
            Assert.Equal("descricao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.Descricao)).ColumnName);
            Assert.Equal("inicio_download", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.InicioDownload)).ColumnName);
            Assert.Equal("inicio", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.Inicio)).ColumnName);
            Assert.Equal("disciplina_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.DisciplinaId)).ColumnName);
            Assert.Equal("disciplina", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.Disciplina)).ColumnName);
            Assert.Equal("fim", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.Fim)).ColumnName);
            Assert.Equal("inclusao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.Inclusao)).ColumnName);
            Assert.Equal("total_itens", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.TotalItens)).ColumnName);
            Assert.Equal("tempo_execucao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.TempoExecucao)).ColumnName);
            Assert.Equal("senha", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.Senha)).ColumnName);
            Assert.Equal("possui_bib", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.PossuiBIB)).ColumnName);
            Assert.Equal("total_cadernos", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.TotalCadernos)).ColumnName);
            Assert.Equal("modalidade", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.Modalidade)).ColumnName);
            Assert.Equal("ocultar_prova", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.OcultarProva)).ColumnName);
            Assert.Equal("aderir_todos", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.AderirTodos)).ColumnName);
            Assert.Equal("multidisciplinar", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.Multidisciplinar)).ColumnName);
            Assert.Equal("tipo_prova_id", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.TipoProvaId)).ColumnName);
            Assert.Equal("formato_tai", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.FormatoTai)).ColumnName);
            Assert.Equal("formato_tai_item", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.ProvaFormatoTaiItem)).ColumnName);
            Assert.Equal("qtd_itens_sincronizacao_respostas", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.QtdItensSincronizacaoRespostas)).ColumnName);
            Assert.Equal("formato_tai_avancar_sem_responder", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.PermiteAvancarSemResponderTai)).ColumnName);
            Assert.Equal("formato_tai_voltar_item_anterior", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.PermiteVoltarItemAnteriorTai)).ColumnName);
            Assert.Equal("ultima_atualizacao", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.UltimaAtualizacao)).ColumnName);
            Assert.Equal("prova_com_proficiencia", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.ProvaComProficiencia)).ColumnName);
            Assert.Equal("apresentar_resultados", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.ApresentarResultados)).ColumnName);
            Assert.Equal("apresentar_resultados_por_item", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.ApresentarResultadosPorItem)).ColumnName);
            Assert.Equal("exibir_video", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.ExibirVideo)).ColumnName);
            Assert.Equal("exibir_audio", map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.ExibirAudio)).ColumnName);
        }

        [Fact]
        public void Deve_Definir_Id_Como_Chave_Primaria()
        {
            var map = (ProvaMap)FluentMapper.EntityMaps[typeof(Prova)];
            var idColumn = map.PropertyMaps.First(p => p.PropertyInfo.Name == nameof(Prova.Id)).ColumnName;
            Assert.Equal("id", idColumn);
        }
    }
}
