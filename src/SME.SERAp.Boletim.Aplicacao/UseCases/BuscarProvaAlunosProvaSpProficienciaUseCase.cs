﻿using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunosProvaProficienciaBoletimPorProvaId;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Exceptions;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class BuscarProvaAlunosProvaSpProficienciaUseCase : AbstractUseCase, IBuscarProvaAlunosProvaSpProficienciaUseCase
    {
        private readonly IServicoLog servicoLog;
        public BuscarProvaAlunosProvaSpProficienciaUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog) : base(mediator, channel)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaId = long.Parse(mensagemRabbit.Mensagem.ToString() ?? string.Empty);
                if (provaId == 0)
                    throw new Exception("O Id da prova deve ser informado.");

                var alunosProvaProficienciaBoletim = await mediator.Send(new ObterAlunosProvaProficienciaBoletimPorProvaIdQuery(provaId));
                if (alunosProvaProficienciaBoletim == null || !alunosProvaProficienciaBoletim.Any())
                    return false;

                foreach (var alunoProvaProficienciaBoletim in alunosProvaProficienciaBoletim)
                {
                    var alunoProvaSpProficiencia = ObterBoletimProvaAluno(alunoProvaProficienciaBoletim);
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.BuscarAlunoProvaSpProficiencia, alunoProvaSpProficiencia));
                }

                return true;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }
        }

        private static BoletimProvaAluno ObterBoletimProvaAluno(AlunoProvaProficienciaBoletimDto alunoProvaProficienciaBoletimDto)
        {
            return new BoletimProvaAluno(alunoProvaProficienciaBoletimDto.CodigoDre, alunoProvaProficienciaBoletimDto.CodigoUe, alunoProvaProficienciaBoletimDto.NomeUe,
                alunoProvaProficienciaBoletimDto.ProvaId, alunoProvaProficienciaBoletimDto.NomeProva, alunoProvaProficienciaBoletimDto.AnoEscolar, alunoProvaProficienciaBoletimDto.Turma,
                alunoProvaProficienciaBoletimDto.CodigoAluno, alunoProvaProficienciaBoletimDto.NomeAluno, alunoProvaProficienciaBoletimDto.NomeDisciplina, alunoProvaProficienciaBoletimDto.DisciplinaId,
                alunoProvaProficienciaBoletimDto.ProvaStatus, alunoProvaProficienciaBoletimDto.Proficiencia, alunoProvaProficienciaBoletimDto.ErroMedida, alunoProvaProficienciaBoletimDto.NivelCodigo);
        }
    }
}
