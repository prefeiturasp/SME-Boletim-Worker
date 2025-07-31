using SME.SERAp.Boletim.Dominio.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Enums
{
    public class ProvaStatusTeste
    {
        [Fact]
        public void ValoresDoEnumDevemEstarCorretos()
        {
            Assert.Equal(0, (int)ProvaStatus.NaoIniciado);
            Assert.Equal(1, (int)ProvaStatus.Iniciado);
            Assert.Equal(2, (int)ProvaStatus.Finalizado);
            Assert.Equal(3, (int)ProvaStatus.Pendente);
            Assert.Equal(4, (int)ProvaStatus.EmRevisao);
            Assert.Equal(5, (int)ProvaStatus.FINALIZADA_AUTOMATICAMENTE_JOB);
            Assert.Equal(6, (int)ProvaStatus.FINALIZADA_AUTOMATICAMENTE_TEMPO);
            Assert.Equal(7, (int)ProvaStatus.FINALIZADA_OFFLINE);
        }

        [Theory]
        [InlineData(ProvaStatus.NaoIniciado, "Não Iniciado")]
        [InlineData(ProvaStatus.Iniciado, "Iniciado")]
        [InlineData(ProvaStatus.Finalizado, "Finalizado")]
        [InlineData(ProvaStatus.Pendente, "Pendente")]
        [InlineData(ProvaStatus.EmRevisao, "Em Revisão")]
        [InlineData(ProvaStatus.FINALIZADA_AUTOMATICAMENTE_JOB, "Finalizado Automaticamente")]
        [InlineData(ProvaStatus.FINALIZADA_AUTOMATICAMENTE_TEMPO, "Finalizado Automaticamente por Tempo")]
        [InlineData(ProvaStatus.FINALIZADA_OFFLINE, "Finalizado Offile")]
        public void DeveTerDisplayNameCorreto(ProvaStatus status, string expectedName)
        {
            var memberInfo = typeof(ProvaStatus).GetMember(status.ToString()).FirstOrDefault();
            var displayAttribute = memberInfo?.GetCustomAttribute<DisplayAttribute>();

            Assert.NotNull(displayAttribute);
            Assert.Equal(expectedName, displayAttribute.Name);
        }
    }
}
