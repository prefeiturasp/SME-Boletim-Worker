using MediatR;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvasFinalizadasPorData;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;
using SME.SERAp.Boletim.Infra.Services;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class BuscaProvasFinalizadasUseCase : AbstractUseCase, IBuscaProvasFinalizadasUseCase
    {
        private readonly IServicoLog servicoLog;
        public BuscaProvasFinalizadasUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var dataProvasFinalizadas = DateTime.Now.Date;
                var provasFinalizadas = await mediator.Send(new ObterProvasFinalizadasPorDataQuery(dataProvasFinalizadas));
                if (provasFinalizadas?.Any() ?? false)
                {
                    foreach (var provaFinalizada in provasFinalizadas)
                    {
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.BuscaAlunosProvaProficienciaBoletim, provaFinalizada));
                    }
                }
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }

            return true;
        }
    }
}
