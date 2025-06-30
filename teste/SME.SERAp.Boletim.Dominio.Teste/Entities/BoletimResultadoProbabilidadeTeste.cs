using SME.SERAp.Boletim.Dominio.Entities;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Entities
{
    public class BoletimResultadoProbabilidadeTeste
    {
        [Fact]
        public void Construtor_DeveAtribuirPropriedadesCorretamente()
        {
            long habilidadeId = 1;
            string codigoHabilidade = "HAB01";
            string habilidadeDescricao = "Resolver problemas matemáticos";
            string turmaDescricao = "6A";
            long turmaId = 10;
            long provaId = 20;
            long ueId = 30;
            long disciplinaId = 40;
            long anoEscolar = 6;
            double abaixoDoBasico = 12.5;
            double basico = 25.0;
            double adequado = 35.0;
            double avancado = 27.5;

            var antes = DateTime.Now;
            var resultado = new BoletimResultadoProbabilidade(
                habilidadeId, codigoHabilidade, habilidadeDescricao, turmaDescricao,
                turmaId, provaId, ueId, disciplinaId, anoEscolar,
                abaixoDoBasico, basico, adequado, avancado
            );
            var depois = DateTime.Now;

            Assert.Equal(habilidadeId, resultado.HabilidadeId);
            Assert.Equal(codigoHabilidade, resultado.CodigoHabilidade);
            Assert.Equal(habilidadeDescricao, resultado.HabilidadeDescricao);
            Assert.Equal(turmaDescricao, resultado.TurmaDescricao);
            Assert.Equal(turmaId, resultado.TurmaId);
            Assert.Equal(provaId, resultado.ProvaId);
            Assert.Equal(ueId, resultado.UeId);
            Assert.Equal(disciplinaId, resultado.DisciplinaId);
            Assert.Equal(anoEscolar, resultado.AnoEscolar);
            Assert.Equal(abaixoDoBasico, resultado.AbaixoDoBasico);
            Assert.Equal(basico, resultado.Basico);
            Assert.Equal(adequado, resultado.Adequado);
            Assert.Equal(avancado, resultado.Avancado);
            Assert.True(resultado.DataConsolidacao >= antes && resultado.DataConsolidacao <= depois);
        }
    }
}
