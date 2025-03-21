﻿using SME.SERAp.Boletim.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dados.Interfaces
{
    public interface IRepositorioBase<T> where T : EntidadeBase
    {
        Task<long> SalvarAsync(T entidade);
        Task<T> ObterPorIdAsync(long id);
        Task<long> IncluirAsync(T entidade);
        Task<long> UpdateAsync(T entidade);
        Task<IEnumerable<T>> ObterTudoAsync();
    }
}