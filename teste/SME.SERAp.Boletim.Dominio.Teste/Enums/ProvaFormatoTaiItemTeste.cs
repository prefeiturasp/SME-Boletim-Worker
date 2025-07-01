using SME.SERAp.Boletim.Dominio.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Enums
{
    public class ProvaFormatoTaiItemTeste
    {
        [Fact]
        public void ValoresDoEnumDevemEstarCorretos()
        {
            Assert.Equal(0, (int)ProvaFormatoTaiItem.Todos);
            Assert.Equal(20, (int)ProvaFormatoTaiItem.Item_20);
            Assert.Equal(30, (int)ProvaFormatoTaiItem.Item_30);
            Assert.Equal(40, (int)ProvaFormatoTaiItem.Item_40);
            Assert.Equal(50, (int)ProvaFormatoTaiItem.Item_50);
        }

        [Theory]
        [InlineData(ProvaFormatoTaiItem.Todos, "Todos")]
        [InlineData(ProvaFormatoTaiItem.Item_20, "20")]
        [InlineData(ProvaFormatoTaiItem.Item_30, "30")]
        [InlineData(ProvaFormatoTaiItem.Item_40, "40")]
        [InlineData(ProvaFormatoTaiItem.Item_50, "50")]
        public void DeveTerDisplayNameCorreto(ProvaFormatoTaiItem item, string expectedName)
        {
            var memberInfo = typeof(ProvaFormatoTaiItem).GetMember(item.ToString()).FirstOrDefault();
            var displayAttribute = memberInfo?.GetCustomAttribute<DisplayAttribute>();

            Assert.NotNull(displayAttribute);
            Assert.Equal(expectedName, displayAttribute.Name);
        }
    }
}
