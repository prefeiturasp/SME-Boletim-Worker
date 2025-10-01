using FluentValidation.Results;
using SME.SERAp.Boletim.Infra.Exceptions;

namespace SME.SERAp.Boletim.Infra.Testes.Exceptions
{
    public class ValidacaoExceptionTeste
    {
        [Fact]
        public void Deve_Atribuir_Erros_Corretamente()
        {
            var erros = new List<ValidationFailure>
            {
                new ValidationFailure("Campo1", "Erro 1"),
                new ValidationFailure("Campo2", "Erro 2")
            };

            var exception = new ValidacaoException(erros);

            Assert.Equal(erros, exception.Erros);
        }

        [Fact]
        public void Deve_Retornar_Mensagens_Corretamente()
        {
            var erros = new List<ValidationFailure>
            {
                new ValidationFailure("Campo1", "Erro 1"),
                new ValidationFailure("Campo2", "Erro 2")
            };

            var exception = new ValidacaoException(erros);

            var mensagens = exception.Mensagens();

            Assert.Equal(new List<string> { "Erro 1", "Erro 2" }, mensagens);
        }

        [Fact]
        public void Mensagens_Deve_Retornar_Lista_Vazia_Se_Erros_Nulo()
        {
            var exception = new ValidacaoException(null);

            var mensagens = exception.Mensagens();

            Assert.Null(mensagens);
        }
    }
}
