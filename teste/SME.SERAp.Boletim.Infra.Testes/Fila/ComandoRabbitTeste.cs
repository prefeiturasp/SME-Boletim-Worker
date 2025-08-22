using SME.SERAp.Boletim.Infra.Fila;

namespace SME.SERAp.Boletim.Infra.Testes.Fila
{
    public class ComandoRabbitTeste
    {
        private class CasoUsoTeste { }

        [Fact]
        public void Deve_Criar_ComandoRabbit_Com_Construtor_Basico()
        {
            var comando = new ComandoRabbit("ProcessoTeste", typeof(CasoUsoTeste));

            Assert.Equal("ProcessoTeste", comando.NomeProcesso);
            Assert.Equal(typeof(CasoUsoTeste), comando.TipoCasoUso);
            Assert.Equal((ulong)3, comando.QuantidadeReprocessamentoDeadLetter);
            Assert.Equal(10 * 60 * 100, comando.Ttl);
            Assert.False(comando.ModeLazy);
        }

        [Fact]
        public void Deve_Criar_ComandoRabbit_Com_Construtor_ModeLazy()
        {
            var comando = new ComandoRabbit("ProcessoTeste", typeof(CasoUsoTeste), true);

            Assert.Equal("ProcessoTeste", comando.NomeProcesso);
            Assert.Equal(typeof(CasoUsoTeste), comando.TipoCasoUso);
            Assert.Equal((ulong)3, comando.QuantidadeReprocessamentoDeadLetter);
            Assert.Equal(10 * 60 * 100, comando.Ttl);
            Assert.True(comando.ModeLazy);
        }

        [Fact]
        public void Deve_Criar_ComandoRabbit_Com_Construtor_Completo()
        {
            var comando = new ComandoRabbit("ProcessoTeste", typeof(CasoUsoTeste), true, 5, 5000);

            Assert.Equal("ProcessoTeste", comando.NomeProcesso);
            Assert.Equal(typeof(CasoUsoTeste), comando.TipoCasoUso);
            Assert.Equal((ulong)5, comando.QuantidadeReprocessamentoDeadLetter);
            Assert.Equal(5000, comando.Ttl);
            Assert.True(comando.ModeLazy);
        }
    }
}
