using SME.SERAp.Boletim.Dominio.Entities;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Entities
{
    public class NivelProficienciaTeste
    {
        [Fact]
        public void DeveAtribuirPropriedadesCorretamente()
        {
            int codigo = 3;
            string descricao = "Adequado";
            long? valorReferencia = 275;
            long disciplinaId = 10;
            long ano = 5;

            var nivel = new NivelProficiencia
            {
                Codigo = codigo,
                Descricao = descricao,
                ValorReferencia = valorReferencia,
                DisciplinaId = disciplinaId,
                Ano = ano
            };

            Assert.Equal(codigo, nivel.Codigo);
            Assert.Equal(descricao, nivel.Descricao);
            Assert.Equal(valorReferencia, nivel.ValorReferencia);
            Assert.Equal(disciplinaId, nivel.DisciplinaId);
            Assert.Equal(ano, nivel.Ano);
        }
    }
}
