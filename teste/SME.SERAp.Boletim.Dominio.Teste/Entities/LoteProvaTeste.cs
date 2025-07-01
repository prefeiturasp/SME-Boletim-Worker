using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Dominio.Enums;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Entities
{
    public class LoteProvaTeste
    {
        [Fact]
        public void Construtor_DeveAtribuirPropriedadesCorretamente()
        {
            string nome = "Lote 1";
            bool tipoTai = true;
            bool exibirNoBoletim = false;
            DateTime dataCorrecaoFim = new DateTime(2025, 6, 1, 12, 0, 0);
            DateTime dataInicioLote = new DateTime(2025, 5, 15, 8, 30, 0);

            var antes = DateTime.Now;
            var lote = new LoteProva(nome, tipoTai, exibirNoBoletim, dataCorrecaoFim, dataInicioLote);
            var depois = DateTime.Now;

            Assert.Equal(nome, lote.Nome);
            Assert.Equal(tipoTai, lote.TipoTai);
            Assert.Equal(exibirNoBoletim, lote.ExibirNoBoletim);
            Assert.Equal(dataCorrecaoFim, lote.DataCorrecaoFim);
            Assert.Equal(dataInicioLote, lote.DataInicioLote);
            Assert.Equal(LoteStatusConsolidacao.NaoConsolidado, lote.StatusConsolidacao);
            Assert.Null(lote.DataAlteracao);
            Assert.True(lote.DataCriacao >= antes && lote.DataCriacao <= depois);
        }
    }
}
