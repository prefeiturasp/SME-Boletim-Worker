using SME.SERAp.Boletim.Dominio.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Enums
{
    public class TipoNivelProficienciaTeste
    {
        [Fact]
        public void ValoresDoEnumDevemEstarCorretos()
        {
            Assert.Equal(1, (int)TipoNivelProficiencia.AbaixoBasico);
            Assert.Equal(2, (int)TipoNivelProficiencia.Basico);
            Assert.Equal(3, (int)TipoNivelProficiencia.Adequado);
            Assert.Equal(4, (int)TipoNivelProficiencia.Avancado);
        }

        [Theory]
        [InlineData(TipoNivelProficiencia.AbaixoBasico, "Abaixo do básico")]
        [InlineData(TipoNivelProficiencia.Basico, "Básico")]
        [InlineData(TipoNivelProficiencia.Adequado, "Adequado")]
        [InlineData(TipoNivelProficiencia.Avancado, "Avançado")]
        public void DeveTerDisplayNameCorreto(TipoNivelProficiencia tipo, string expectedName)
        {
            var memberInfo = typeof(TipoNivelProficiencia).GetMember(tipo.ToString()).FirstOrDefault();
            var displayAttribute = memberInfo?.GetCustomAttribute<DisplayAttribute>();

            Assert.NotNull(displayAttribute);
            Assert.Equal(expectedName, displayAttribute.Name);
        }
    }
}
