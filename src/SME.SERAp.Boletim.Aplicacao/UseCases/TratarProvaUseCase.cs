using MediatR;
using SME.SERAp.Boletim.Aplicacao.Commands.Prova.Alterar;
using SME.SERAp.Boletim.Aplicacao.Commands.Prova.Inserir;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaPorId;
using SME.SERAp.Boletim.Infra.Dtos.SerapEstudantes;
using SME.SERAp.Boletim.Infra.Fila;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class TratarProvaUseCase : AbstractUseCase, ITratarProvaUseCase
    {
        public TratarProvaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provaDto = mensagemRabbit.ObterObjetoMensagem<ProvaDto>();
            if (provaDto == null) return false;

            var prova = await mediator.Send(new ObterProvaPorIdQuery(provaDto.Id));
            if (prova == null)
            {
                await mediator.Send(new InserirProvaCommand(provaDto));
            }
            else if (prova.Codigo != provaDto.Codigo ||
                     prova.Descricao != provaDto.Descricao ||
                     prova.Modalidade != provaDto.Modalidade ||
                     prova.Inicio != provaDto.Inicio ||
                     prova.Fim != provaDto.Fim)
            {
                await mediator.Send(new AlterarProvaCommand(provaDto));
            }

            return true;
        }
    }
}