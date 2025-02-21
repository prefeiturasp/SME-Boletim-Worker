﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Fila
{
    public class ComandoRabbit
    {
        public ComandoRabbit(string nomeProcesso, Type tipoCasoUso, ulong quantidadeReprocessamentoDeadLetter = 3,
            int ttl = 10 * 60 * 100)
        {
            NomeProcesso = nomeProcesso;
            TipoCasoUso = tipoCasoUso;
            QuantidadeReprocessamentoDeadLetter = quantidadeReprocessamentoDeadLetter;
            Ttl = ttl;
        }

        public ComandoRabbit(string nomeProcesso, Type tipoCasoUso, bool modeLazy) : this(nomeProcesso, tipoCasoUso)
        {
            ModeLazy = modeLazy;
        }

        public ComandoRabbit(string nomeProcesso, Type tipoCasoUso, bool modeLazy, ulong quantidadeReprocessamentoDeadLetter,
            int ttl) : this(nomeProcesso, tipoCasoUso, quantidadeReprocessamentoDeadLetter, ttl)
        {
            ModeLazy = modeLazy;
        }

        public string NomeProcesso { get; }
        public Type TipoCasoUso { get; }
        public ulong QuantidadeReprocessamentoDeadLetter { get; }
        public int Ttl { get; }
        public bool ModeLazy { get; }
    }
}