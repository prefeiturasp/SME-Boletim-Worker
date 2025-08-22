using SME.SERAp.Boletim.Infra.Exceptions;
using System.Net;

namespace SME.SERAp.Boletim.Infra.Testes.Exceptions
{
    public class NegocioExceptionTeste
    {
        [Fact]
        public void Deve_Criar_NegocioException_Com_StatusCode_Default()
        {
            var exception = new NegocioException("mensagem de erro de negócio");

            Assert.Equal("mensagem de erro de negócio", exception.Message);
            Assert.Equal(409, exception.StatusCode);
        }

        [Fact]
        public void Deve_Criar_NegocioException_Com_StatusCode_Int()
        {
            var exception = new NegocioException("mensagem de erro de negócio", 400);

            Assert.Equal("mensagem de erro de negócio", exception.Message);
            Assert.Equal(400, exception.StatusCode);
        }

        [Fact]
        public void Deve_Criar_NegocioException_Com_StatusCode_HttpStatusCode()
        {
            var exception = new NegocioException("mensagem de erro de negócio", HttpStatusCode.Forbidden);

            Assert.Equal("mensagem de erro de negócio", exception.Message);
            Assert.Equal((int)HttpStatusCode.Forbidden, exception.StatusCode);
        }
    }
}
