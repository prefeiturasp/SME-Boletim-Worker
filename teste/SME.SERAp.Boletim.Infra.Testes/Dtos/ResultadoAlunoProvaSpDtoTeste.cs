using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos
{
    public class ResultadoAlunoProvaSpDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente()
        {
            var dto = new ResultadoAlunoProvaSpDto();

            var edicao = "2024";
            var areaConhecimentoId = 10;
            var anoEscolar = "9º Ano";
            var alunoMatricula = "123456";
            var valor = "85.5";

            dto.Edicao = edicao;
            dto.AreaConhecimentoID = areaConhecimentoId;
            dto.AnoEscolar = anoEscolar;
            dto.AlunoMatricula = alunoMatricula;
            dto.Valor = valor;

            Assert.Equal(edicao, dto.Edicao);
            Assert.Equal(areaConhecimentoId, dto.AreaConhecimentoID);
            Assert.Equal(anoEscolar, dto.AnoEscolar);
            Assert.Equal(alunoMatricula, dto.AlunoMatricula);
            Assert.Equal(valor, dto.Valor);
        }
    }
}
