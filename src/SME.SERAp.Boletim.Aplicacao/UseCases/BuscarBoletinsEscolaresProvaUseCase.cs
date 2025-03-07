using MediatR;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresDetalhesPorProvaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterBoletinsEscolaresPorProvaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaPorId;
using SME.SERAp.Boletim.Infra.Exceptions;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class BuscarBoletinsEscolaresProvaUseCase : AbstractUseCase, IBuscarBoletinsEscolaresProvaUseCase
    {
        private readonly IServicoLog servicoLog;

        public BuscarBoletinsEscolaresProvaUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaId = long.Parse(mensagemRabbit.Mensagem.ToString() ?? string.Empty);
                if (provaId == 0)
                    throw new NegocioException("O Id da prova deve ser informado.");

                var prova = await mediator.Send(new ObterProvaPorIdQuery(provaId));
                if (prova is null)
                    throw new NegocioException($"O prova {provaId} não encontrada.");

                var provaBoletimDetalhesDtos = await mediator.Send(new ObterBoletinsEscolaresDetalhesPorProvaIdQuery(prova.Id));
                if (provaBoletimDetalhesDtos?.Any() ?? false)
                {
                    await RemoverBoletinsEscolarExistentesParaReinsercao(provaId);

                    foreach (var provaBoletimDetalhesDto in provaBoletimDetalhesDtos)
                    {
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarBoletimEscolarProva, provaBoletimDetalhesDto));
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

        private async Task RemoverBoletinsEscolarExistentesParaReinsercao(long provaId)
        {
            var boletinsEscolarExistentes = await mediator.Send(new ObterBoletinsEscolaresPorProvaIdQuery(provaId));
            if (boletinsEscolarExistentes?.Any() ?? false)
            {
                await mediator.Send(new ExcluirBoletinsEscolaresPorProvaIdCommand(provaId));
            }
        }
    }
}
