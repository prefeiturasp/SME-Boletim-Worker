using SME.SERAp.Boletim.Infra.Fila;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Interfaces
{
    public interface IUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}