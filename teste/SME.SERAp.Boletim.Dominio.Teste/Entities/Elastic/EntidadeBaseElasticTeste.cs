using SME.SERAp.Boletim.Dominio.Entities.Elastic;

namespace SME.SERAp.Boletim.Dominio.Teste.Entities.Elastic
{
    public class EntidadeBaseElasticTeste
    {
        private class EntidadeBaseElasticFake : EntidadeBaseElastic { }

        [Fact]
        public void DevePermitirDefinirEObterId()
        {
            var entidade = new EntidadeBaseElasticFake();

            entidade.Id = "123";

            Assert.Equal("123", entidade.Id);
        }

        [Fact]
        public void Id_DevePermitirValorNulo()
        {
            var entidade = new EntidadeBaseElasticFake();

            entidade.Id = null;

            Assert.Null(entidade.Id);
        }
    }
}
