using SME.SERAp.Boletim.Dominio.Entities;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Entities
{
    public class BoletimResultadoTeste
    {
        [Fact]
        public void DeveAtribuirPropriedadesCorretamente()
        {
            long codDre = 1;
            long codUe = 2;
            string nomeUe = "Escola Teste";
            long idProva = 3;
            string nomeProva = "Prova Brasil";
            int anoEscolar = 5;
            string turma = "5B";
            long eolAluno = 123456;
            string nomeAluno = "Maria Oliveira";
            string componente = "Português";
            string statusProva = "Finalizada";
            decimal proficiencia = 245.6m;
            decimal erroMedida = 2.1m;
            string nivel = "Avançado";

            var resultado = new BoletimResultado
            {
                CodDre = codDre,
                CodUe = codUe,
                NomeUe = nomeUe,
                IdProva = idProva,
                NomeProva = nomeProva,
                AnoEscolar = anoEscolar,
                Turma = turma,
                EolAluno = eolAluno,
                NomeAluno = nomeAluno,
                Componente = componente,
                StatusProva = statusProva,
                Proficiencia = proficiencia,
                ErroMedida = erroMedida,
                Nivel = nivel
            };

            Assert.Equal(codDre, resultado.CodDre);
            Assert.Equal(codUe, resultado.CodUe);
            Assert.Equal(nomeUe, resultado.NomeUe);
            Assert.Equal(idProva, resultado.IdProva);
            Assert.Equal(nomeProva, resultado.NomeProva);
            Assert.Equal(anoEscolar, resultado.AnoEscolar);
            Assert.Equal(turma, resultado.Turma);
            Assert.Equal(eolAluno, resultado.EolAluno);
            Assert.Equal(nomeAluno, resultado.NomeAluno);
            Assert.Equal(componente, resultado.Componente);
            Assert.Equal(statusProva, resultado.StatusProva);
            Assert.Equal(proficiencia, resultado.Proficiencia);
            Assert.Equal(erroMedida, resultado.ErroMedida);
            Assert.Equal(nivel, resultado.Nivel);
        }
    }
}
