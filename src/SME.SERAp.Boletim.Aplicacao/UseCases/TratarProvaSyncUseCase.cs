using MediatR;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.SerapEstudantes.ObterTodasProvasSerap;
using SME.SERAp.Boletim.Infra.Fila;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class TratarProvaSyncUseCase : AbstractUseCase, ITratarProvaSyncUseCase
    {
        public TratarProvaSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provas = await mediator.Send(new ObterTodasProvasSerapQuery());

            if (provas != null && provas.Any())
            {
                foreach (var prova in provas)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaTratar, prova));
                }
            }

            return true;
        }
    }
}