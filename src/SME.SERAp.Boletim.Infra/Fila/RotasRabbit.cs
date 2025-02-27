namespace SME.SERAp.Boletim.Infra.Fila
{
    public class RotasRabbit
    {
        public static string RotaLogs => "ApplicationLog";
        public static string Log => "ApplicationLog";

        public const string IniciarSync = "serap.boletim.iniciar.sync";

        public const string BuscaProvasFinalizadas = "serap.boletim.busca.provas.finalizadas";
        public const string BuscaAlunosProvaProficienciaBoletim = "serap.boletim.busca.alunos.prova.proficiencia.boletim";
        public const string TrataBoletimProvaAluno = "serap.boletim.tratar.boletim.prova.aluno";
    }
}