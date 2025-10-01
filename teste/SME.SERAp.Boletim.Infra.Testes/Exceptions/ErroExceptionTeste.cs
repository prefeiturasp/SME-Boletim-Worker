using SME.SERAp.Boletim.Infra.Exceptions;
using System.Net;

namespace SME.SERAp.Boletim.Infra.Testes.Exceptions
{
    public class ErroExceptionTeste
    {
        [Fact]
        public void Deve_Criar_ErroException_Com_StatusCode_Default()
        {
            var exception = new ErroException("mensagem de erro");

            Assert.Equal("mensagem de erro", exception.Message);
            Assert.Equal(500, exception.StatusCode);
        }

        [Fact]
        public void Deve_Criar_ErroException_Com_StatusCode_Int()
        {
            var exception = new ErroException("mensagem de erro", 404);

            Assert.Equal("mensagem de erro", exception.Message);
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public void Deve_Criar_ErroException_Com_StatusCode_HttpStatusCode()
        {
            var exception = new ErroException("mensagem de erro", HttpStatusCode.BadRequest);

            Assert.Equal("mensagem de erro", exception.Message);
            Assert.Equal((int)HttpStatusCode.BadRequest, exception.StatusCode);
        }
    }
}
