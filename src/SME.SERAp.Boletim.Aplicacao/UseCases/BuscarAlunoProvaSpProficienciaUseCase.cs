using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Extensions;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class BuscarAlunoProvaSpProficienciaUseCase : AbstractUseCase, IBuscarAlunoProvaSpProficienciaUseCase
    {
        private readonly IServicoLog servicoLog;

        public BuscarAlunoProvaSpProficienciaUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog) : base(mediator, channel)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var boletimProvaAluno = mensagemRabbit.ObterObjetoMensagem<BoletimProvaAluno>();
                if (boletimProvaAluno is null)
                    throw new Exception("Mensagem inválida.");

                var areaDoConhecimentoId = ObterAreaDoConhecimento(boletimProvaAluno.DisciplinaId);
                var alunoMatricula = boletimProvaAluno.AlunoRa.ToString();
                var edicaoProvaSp = await ObterEdicaoProvaSp(boletimProvaAluno.ProvaId);

                var ResultadoAlunoProvaSp = await mediator
                    .Send(new ObterResultadoAlunoProvaSpQuery(edicaoProvaSp, areaDoConhecimentoId, alunoMatricula));

                if (ResultadoAlunoProvaSp is null)
                    return true;

                var anoEscolar = ResultadoAlunoProvaSp.AnoEscolar?.ConverterParaInt() ?? 0;
                var anoLetivo = ResultadoAlunoProvaSp.Edicao?.ConverterParaInt() ?? 0;

                var alunoProvaSpProficiencia = new AlunoProvaSpProficiencia
                {
                    AlunoRa = boletimProvaAluno.AlunoRa,
                    DisciplinaId = boletimProvaAluno.DisciplinaId,
                    AnoEscolar = anoEscolar,
                    AnoLetivo = anoLetivo,
                    NivelProficiencia = ResultadoAlunoProvaSp.NivelProficiencia,
                    Proficiencia = ResultadoAlunoProvaSp.Valor,
                    DataAtualizacao = DateTime.Now
                };

                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarAlunoProvaSpProficiencia, alunoProvaSpProficiencia));

                return true;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }
        }

        private async Task<int> ObterEdicaoProvaSp(long provaId)
        {
            var anoProva = await mediator.Send(new ObterAnoProvaQuery(provaId));
            if(anoProva is null || anoProva == 0)
                throw new Exception($"Não foi possível identificar o ano da prova. ProvaId: {provaId}");

            var edicao = anoProva.Value - 1;
            return edicao;
        }

        private int ObterAreaDoConhecimento(long disciplinaId)
        {
            return disciplinaId switch
            {
                //Matemática
                4 => 3,
                //Língua Portuguesa
                5 => 2,
                //Ciências
                2 or 6 or 7 => 1,
                _ => 0,
            };
        }
    }
}
