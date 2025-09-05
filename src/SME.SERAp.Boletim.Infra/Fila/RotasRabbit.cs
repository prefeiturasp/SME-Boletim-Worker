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

        public const string BuscarBoletimLoteUe = "serap.boletim.buscar.boletim.lote.ue";
        public const string TratarBoletimLoteUe = "serap.boletim.tratar.boletim.lote.ue";

        public const string BuscarProvasUesTotalAlunosAcompanhamento = "serap.boletim.buscar.provas.ues.total.alunos.acompanhamento";
        public const string TratarProvasUesTotalAlunosAcompanhamento = "serap.boletim.tratar.provas.ues.total.alunos.acompanhamento";

        public const string BuscarTurmasBoletimResultadoProbabilidadeProva = "serap.boletim.buscar.turmas.resultado.probabilidade.prova";
        public const string TratarTurmaBoletimResultadoProbabilidadeProva = "serap.boletim.tratar.turma.resultado.probabilidade.prova";
        public const string TratarBoletimResultadoProbabilidadeProva = "serap.boletim.tratar.resultado.probabilidade.prova";

        public const string BuscarAlunoProvaSpProficiencia = "serap.boletim.buscar.aluno.prova.sp.proficiencia";
        public const string TratarAlunoProvaSpProficiencia = "serap.boletim.tratar.aluno.prova.sp.proficiencia";

        public const string BuscarProvaAlunosProvaSpProficiencia = "serap.boletim.buscar.prova.alunos.prova.sp.proficiencia";
    }
}