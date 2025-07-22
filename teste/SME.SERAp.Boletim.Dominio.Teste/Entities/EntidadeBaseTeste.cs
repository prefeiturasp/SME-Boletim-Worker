using SME.SERAp.Boletim.Dominio.Entities;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Entities
{
    public class EntidadeBaseTeste
    {
        private class EntidadeFake : EntidadeBase { }

        [Fact]
        public void DevePermitirAtribuirEObterId()
        {
            var entidade = new EntidadeFake { Id = 123 };

            Assert.Equal(123, entidade.Id);
        }
    }
}
