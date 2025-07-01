using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Dominio.Enums;
using Xunit;

namespace SME.SERAp.Boletim.Dominio.Teste.Entities
{
    public class BoletimProvaAlunoTeste
    {
        [Fact]
        public void Construtor_DeveAtribuirPropriedadesCorretamente()
        {
            long dreId = 1;
            string codigoUe = "UE123";
            string nomeUe = "Escola Municipal";
            long provaId = 10;
            string provaDescricao = "Prova SAEB";
            int anoEscolar = 5;
            string turma = "5A";
            long alunoRa = 123456789;
            string alunoNome = "João da Silva";
            string disciplina = "Matemática";
            long disciplinaId = 99;
            ProvaStatus provaStatus = ProvaStatus.Finalizado;
            decimal proficiencia = 250.5m;
            decimal erroMedida = 3.2m;
            long nivelCodigo = 7;

            var boletim = new BoletimProvaAluno(
                dreId, codigoUe, nomeUe, provaId,
                provaDescricao, anoEscolar, turma, alunoRa, alunoNome,
                disciplina, disciplinaId, provaStatus, proficiencia,
                erroMedida, nivelCodigo
            );

            Assert.Equal(dreId, boletim.DreId);
            Assert.Equal(codigoUe, boletim.CodigoUe);
            Assert.Equal(nomeUe, boletim.NomeUe);
            Assert.Equal(provaId, boletim.ProvaId);
            Assert.Equal(provaDescricao, boletim.ProvaDescricao);
            Assert.Equal(anoEscolar, boletim.AnoEscolar);
            Assert.Equal(turma, boletim.Turma);
            Assert.Equal(alunoRa, boletim.AlunoRa);
            Assert.Equal(alunoNome, boletim.AlunoNome);
            Assert.Equal(disciplina, boletim.Disciplina);
            Assert.Equal(disciplinaId, boletim.DisciplinaId);
            Assert.Equal(provaStatus, boletim.ProvaStatus);
            Assert.Equal(proficiencia, boletim.Proficiencia);
            Assert.Equal(erroMedida, boletim.ErroMedida);
            Assert.Equal(nivelCodigo, boletim.NivelCodigo);
        }
    }
}
