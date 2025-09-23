using SME.SERAp.Boletim.Infra.Extensions;
using SME.SERAp.Boletim.Infra.Fila;

namespace SME.SERAp.Boletim.Infra.Testes.Fila
{
    public class MensagemRabbitTeste
    {
        private class MensagemRabbitFake : MensagemRabbit
        {
            public MensagemRabbitFake() : base()
            {
            }
        }

        private class ClasseTeste
        {
            public int Id { get; set; }
            public string Nome { get; set; }
        }

        [Fact]
        public void Deve_Criar_MensagemRabbit_Corretamente()
        {
            var codigo = Guid.NewGuid();
            var mensagem = new { Texto = "Teste" };

            var mensagemRabbit = new MensagemRabbit(mensagem, codigo);

            Assert.Equal(mensagem, mensagemRabbit.Mensagem);
            Assert.Equal(codigo, mensagemRabbit.CodigoCorrelacao);
        }

        [Fact]
        public void Deve_Permitir_Criar_Instancia_Com_Construtor_Protegido()
        {
            var instancia = new MensagemRabbitFake();

            Assert.NotNull(instancia);
            Assert.Null(instancia.Mensagem);
            Assert.Equal(Guid.Empty, instancia.CodigoCorrelacao);
        }

        [Fact]
        public void Deve_ObterObjetoMensagem_Corretamente()
        {
            var obj = new ClasseTeste { Id = 1, Nome = "Teste" };
            var json = obj.ConverterObjectParaJson();
            var codigo = Guid.NewGuid();

            var mensagemRabbit = new MensagemRabbit(json, codigo);
            var resultado = mensagemRabbit.ObterObjetoMensagem<ClasseTeste>();

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal("Teste", resultado.Nome);
        }

        [Fact]
        public void Deve_Retornar_Nulo_Se_Mensagem_Nula()
        {
            var codigo = Guid.NewGuid();
            var mensagemRabbit = new MensagemRabbit(null, codigo);

            var resultado = mensagemRabbit.ObterObjetoMensagem<ClasseTeste>();

            Assert.Null(resultado);
        }
    }
}
