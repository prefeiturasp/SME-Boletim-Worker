using SME.SERAp.Boletim.Infra.Fila;

namespace SME.SERAp.Boletim.Infra.Testes.Fila
{
    public class RotasRabbitTeste
    {
        [Fact]
        public void Deve_Retornar_Valores_Das_Constantes_Corretamente()
        {
            Assert.Equal("ApplicationLog", RotasRabbit.RotaLogs);
            Assert.Equal("ApplicationLog", RotasRabbit.Log);

            Assert.Equal("serap.boletim.iniciar.sync", RotasRabbit.IniciarSync);
            Assert.Equal("serap.boletim.buscar.provas.finalizadas", RotasRabbit.BuscarProvasFinalizadas);
            Assert.Equal("serap.boletim.buscar.alunos.prova.proficiencia.boletim", RotasRabbit.BuscarAlunosProvaProficienciaBoletim);
            Assert.Equal("serap.boletim.tratar.boletim.prova.aluno", RotasRabbit.TratarBoletimProvaAluno);
            Assert.Equal("serap.boletim.buscar.provas.boletim.lote", RotasRabbit.BuscarProvasBoletimLote);
            Assert.Equal("serap.boletim.buscar.boletim.escolar.prova", RotasRabbit.BuscarBoletimEscolarProva);
            Assert.Equal("serap.boletim.tratar.boletim.escolar.prova", RotasRabbit.TratarBoletimEscolarProva);

            Assert.Equal("serap.boletim.buscar.boletim.lote.ue", RotasRabbit.BuscarBoletimLoteUe);
            Assert.Equal("serap.boletim.tratar.boletim.lote.ue", RotasRabbit.TratarBoletimLoteUe);

            Assert.Equal("serap.boletim.buscar.provas.ues.total.alunos.acompanhamento", RotasRabbit.BuscarProvasUesTotalAlunosAcompanhamento);
            Assert.Equal("serap.boletim.tratar.provas.ues.total.alunos.acompanhamento", RotasRabbit.TratarProvasUesTotalAlunosAcompanhamento);

            Assert.Equal("serap.boletim.buscar.turmas.resultado.probabilidade.prova", RotasRabbit.BuscarTurmasBoletimResultadoProbabilidadeProva);
            Assert.Equal("serap.boletim.tratar.turma.resultado.probabilidade.prova", RotasRabbit.TratarTurmaBoletimResultadoProbabilidadeProva);
            Assert.Equal("serap.boletim.tratar.resultado.probabilidade.prova", RotasRabbit.TratarBoletimResultadoProbabilidadeProva);
        }
    }
}
