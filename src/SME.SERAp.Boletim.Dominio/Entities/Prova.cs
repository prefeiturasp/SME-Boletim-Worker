using SME.SERAp.Boletim.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dominio.Entities
{
    public class Prova : EntidadeBase
    {
        public Prova() { }
        public Prova(long id, long codigo, string descricao, Modalidade modalidade, int ano, DateTime inicio, DateTime fim)
        {
            Id = id;
            Codigo = codigo;
            Descricao = descricao;
            Modalidade = modalidade;
            Ano = ano;
            Inicio = inicio;
            Fim = fim;
        }

        public long Codigo { get; set; }
        public string Descricao { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Ano { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
    }
}