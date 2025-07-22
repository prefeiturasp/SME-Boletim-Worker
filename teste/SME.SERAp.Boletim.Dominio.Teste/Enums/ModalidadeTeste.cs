using SME.SERAp.Boletim.Dominio.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Enums
{
    public class ModalidadeTeste
    {
        [Fact]
        public void ValoresDoEnumDevemEstarCorretos()
        {
            Assert.Equal(0, (int)Modalidade.NaoCadastrado);
            Assert.Equal(1, (int)Modalidade.EducacaoInfantil);
            Assert.Equal(3, (int)Modalidade.EJA);
            Assert.Equal(4, (int)Modalidade.CIEJA);
            Assert.Equal(5, (int)Modalidade.Fundamental);
            Assert.Equal(6, (int)Modalidade.Medio);
            Assert.Equal(7, (int)Modalidade.CMCT);
            Assert.Equal(8, (int)Modalidade.MOVA);
            Assert.Equal(9, (int)Modalidade.ETEC);
        }

        [Theory]
        [InlineData(Modalidade.EducacaoInfantil, "Educação Infantil", "EI")]
        [InlineData(Modalidade.EJA, "Educação de Jovens e Adultos", "EJA")]
        [InlineData(Modalidade.CIEJA, "CIEJA", "CIEJA")]
        [InlineData(Modalidade.Fundamental, "Ensino Fundamental", "EF")]
        [InlineData(Modalidade.Medio, "Ensino Médio", "EM")]
        [InlineData(Modalidade.CMCT, "CMCT", "CMCT")]
        [InlineData(Modalidade.MOVA, "MOVA", "MOVA")]
        [InlineData(Modalidade.ETEC, "ETEC", "ETEC")]
        public void DeveTerDisplayNameEShortNameCorretos(Modalidade modalidade, string expectedName, string expectedShortName)
        {
            var memberInfo = typeof(Modalidade).GetMember(modalidade.ToString()).FirstOrDefault();
            var displayAttribute = memberInfo?.GetCustomAttribute<DisplayAttribute>();

            Assert.NotNull(displayAttribute);
            Assert.Equal(expectedName, displayAttribute.Name);
            Assert.Equal(expectedShortName, displayAttribute.ShortName);
        }
    }
}
