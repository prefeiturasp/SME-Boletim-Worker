using Dapper.FluentMap;
using SME.SERAp.Boletim.Dados.Mapeamentos;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Dados.Testes.Mapeamentos
{
    public class RegistrarMapeamentosTeste
    {
        [Fact]
        public void Deve_Registrar_Todos_Os_Maps_Sem_Erro()
        {

            FluentMapper.EntityMaps.Clear();
            var ex = Record.Exception(() => RegistrarMapeamentos.Registrar());

            Assert.Null(ex);
        }

        [Fact]
        public void Deve_Registrar_Todos_Os_Maps_Corretamente()
        {

            FluentMapper.EntityMaps.Clear();
            RegistrarMapeamentos.Registrar();

            var tiposEsperados = new Type[]
            {
                typeof(Prova),
                typeof(BoletimProvaAluno),
                typeof(AlunoProvaProficiencia),
                typeof(BoletimEscolar),
                typeof(BoletimLoteProva),
                typeof(LoteProva),
                typeof(BoletimResultadoProbabilidade),
                typeof(NivelProficiencia),
                typeof(BoletimLoteUe)
            };

            foreach (var tipo in tiposEsperados)
            {
                Assert.Contains(tipo, FluentMapper.EntityMaps.Keys);
            }
        }
    }
}
