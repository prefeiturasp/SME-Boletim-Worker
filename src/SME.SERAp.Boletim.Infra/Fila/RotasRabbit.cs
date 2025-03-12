namespace SME.SERAp.Boletim.Infra.Fila
{
    public class RotasRabbit
    {
        public static string RotaLogs => "ApplicationLog";
        public static string Log => "ApplicationLog";

        public const string IniciarSync = "serap.boletim.iniciar.sync";

        public const string BuscarProvasFinalizadas = "serap.boletim.buscar.provas.finalizadas";
        public const string BuscarAlunosProvaProficienciaBoletim = "serap.boletim.buscar.alunos.prova.proficiencia.boletim";
        public const string TratarBoletimProvaAluno = "serap.boletim.tratar.boletim.prova.aluno";
        public const string BuscarProvasBoletimLote = "serap.boletim.buscar.provas.boletim.lote";
        public const string BuscarBoletimEscolarProva = "serap.boletim.buscar.boletim.escolar.prova";
        public const string TratarBoletimEscolarProva = "serap.boletim.tratar.boletim.escolar.prova";
    }
}