using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class TrataBoletimResultadoProbabilidadeProvaUseCase : AbstractUseCase, ITrataBoletimResultadoProbabilidadeProvaUseCase
    {
        private readonly IServicoLog servicoLog;

        public TrataBoletimResultadoProbabilidadeProvaUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog) : base(mediator, channel)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaBoletimResultadoProbabilidadeDto = mensagemRabbit.ObterObjetoMensagem<BoletimResultadoProbabilidadeDto>();
                if (provaBoletimResultadoProbabilidadeDto is null) return true;

                var boletimEscolar = ObterBoletimResultadoProbabilidade(provaBoletimResultadoProbabilidadeDto);
                await mediator.Send(new InserirBoletimResultadoProbabilidadeCommand(boletimEscolar));
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }

            return true;
        }

        private static BoletimResultadoProbabilidade ObterBoletimResultadoProbabilidade(BoletimResultadoProbabilidadeDto boletimResultadoProbabilidadeDto)
        {
            return new BoletimResultadoProbabilidade(boletimResultadoProbabilidadeDto.HabilidadeId, boletimResultadoProbabilidadeDto.CodigoHabilidade, boletimResultadoProbabilidadeDto.HabilidadeDescricao, 
                boletimResultadoProbabilidadeDto.TurmaDescricao, boletimResultadoProbabilidadeDto.TurmaId, boletimResultadoProbabilidadeDto.ProvaId, boletimResultadoProbabilidadeDto.UeId,
                boletimResultadoProbabilidadeDto.DisciplinaId, boletimResultadoProbabilidadeDto.AnoEscolar, boletimResultadoProbabilidadeDto.AbaixoDoBasico, boletimResultadoProbabilidadeDto.Basico, 
                boletimResultadoProbabilidadeDto.Adequado, boletimResultadoProbabilidadeDto.Avancado);
        }
    }
}
