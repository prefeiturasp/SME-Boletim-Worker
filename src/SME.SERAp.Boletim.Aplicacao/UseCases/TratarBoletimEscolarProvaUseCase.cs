using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class TratarBoletimEscolarProvaUseCase : AbstractUseCase, ITratarBoletimEscolarProvaUseCase
    {
        private readonly IServicoLog servicoLog;

        public TratarBoletimEscolarProvaUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog) : base(mediator, channel)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaBoletimDetalhesDto = mensagemRabbit.ObterObjetoMensagem<BoletimEscolarDetalhesDto>();
                if (provaBoletimDetalhesDto is null) return true;

                var boletimEscolar = ObterBoletimEscolar(provaBoletimDetalhesDto);
                await mediator.Send(new InserirBoletimEscolarCommand(boletimEscolar));
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }

            return true;
        }

        private static BoletimEscolar ObterBoletimEscolar(BoletimEscolarDetalhesDto boletimEscolarDetalhesDto)
        {
            return new BoletimEscolar(boletimEscolarDetalhesDto.UeId, boletimEscolarDetalhesDto.ProvaId, boletimEscolarDetalhesDto.ComponenteCurricular, boletimEscolarDetalhesDto.DisciplinaId,
                boletimEscolarDetalhesDto.AbaixoBasico, boletimEscolarDetalhesDto.AbaixoBasicoPorcentagem, boletimEscolarDetalhesDto.Basico, boletimEscolarDetalhesDto.BasicoPorcentagem,
                boletimEscolarDetalhesDto.Adequado, boletimEscolarDetalhesDto.AdequadoPorcentagem, boletimEscolarDetalhesDto.Avancado, boletimEscolarDetalhesDto.AvancadoPorcentagem,
                boletimEscolarDetalhesDto.Total, boletimEscolarDetalhesDto.MediaProficiencia, boletimEscolarDetalhesDto.NivelUeCodigo, boletimEscolarDetalhesDto.NivelUeDescricao);
        }
    }
}
