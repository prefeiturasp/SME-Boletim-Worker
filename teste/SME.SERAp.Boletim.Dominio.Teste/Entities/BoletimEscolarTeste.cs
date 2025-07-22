using SME.SERAp.Boletim.Dominio.Entities;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Entities
{
    public class BoletimEscolarTeste
    {
        [Fact]
        public void Construtor_DeveAtribuirPropriedadesCorretamente()
        {
            long ueId = 1;
            long provaId = 2;
            string componenteCurricular = "Matemática";
            long disciplinaId = 3;
            decimal abaixoBasico = 10.5m;
            decimal abaixoBasicoPorcentagem = 20.1m;
            decimal basico = 15.2m;
            decimal basicoPorcentagem = 30.3m;
            decimal adequado = 25.4m;
            decimal adequadoPorcentagem = 40.2m;
            decimal avancado = 35.1m;
            decimal avancadoPorcentagem = 9.4m;
            int total = 100;
            decimal mediaProficiencia = 250.75m;
            int nivelUeCodigo = 1;
            string nivelUeDescricao = "Nível 1";

            var boletim = new BoletimEscolar(
                ueId, provaId, componenteCurricular, disciplinaId,
                abaixoBasico, abaixoBasicoPorcentagem,
                basico, basicoPorcentagem,
                adequado, adequadoPorcentagem,
                avancado, avancadoPorcentagem,
                total, mediaProficiencia, 
                nivelUeCodigo, nivelUeDescricao
            );

            Assert.Equal(ueId, boletim.UeId);
            Assert.Equal(provaId, boletim.ProvaId);
            Assert.Equal(componenteCurricular, boletim.ComponenteCurricular);
            Assert.Equal(disciplinaId, boletim.DisciplinaId);
            Assert.Equal(abaixoBasico, boletim.AbaixoBasico);
            Assert.Equal(abaixoBasicoPorcentagem, boletim.AbaixoBasicoPorcentagem);
            Assert.Equal(basico, boletim.Basico);
            Assert.Equal(basicoPorcentagem, boletim.BasicoPorcentagem);
            Assert.Equal(adequado, boletim.Adequado);
            Assert.Equal(adequadoPorcentagem, boletim.AdequadoPorcentagem);
            Assert.Equal(avancado, boletim.Avancado);
            Assert.Equal(avancadoPorcentagem, boletim.AvancadoPorcentagem);
            Assert.Equal(total, boletim.Total);
            Assert.Equal(mediaProficiencia, boletim.MediaProficiencia);
            Assert.Equal(nivelUeCodigo, boletim.NivelUeCodigo);
            Assert.Equal(nivelUeDescricao, boletim.NivelUeDescricao);
        }
    }
}
