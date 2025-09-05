using SME.SERAp.Boletim.Infra.Fila;

namespace SME.SERAp.Boletim.Infra.Testes.Fila
{
    public class ExchangeRabbitTeste
    {
        [Fact]
        public void Deve_Retornar_Valores_Das_Constantes_Corretamente()
        {
            Assert.Equal("serap.boletim.workers", ExchangeRabbit.SerapBoletim);
            Assert.Equal("serap.workers", ExchangeRabbit.Serap);
            Assert.Equal("serap.boletim.workers.deadletter", ExchangeRabbit.SerapBoletimDeadLetter);
            Assert.Equal("EnterpriseApplicationLog", ExchangeRabbit.Logs);
            Assert.Equal(10 * 60 * 1000, ExchangeRabbit.SerapDeadLetterTtl);
            Assert.Equal(3 * 60 * 1000, ExchangeRabbit.SerapDeadLetterTtl_3);
        }
    }
}
