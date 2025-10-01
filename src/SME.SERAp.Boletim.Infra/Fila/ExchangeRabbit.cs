using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Fila
{
    public static class ExchangeRabbit
    {
        public static string SerapBoletim => "serap.boletim.workers";
        public static string SerapEstudante => "serap.estudante.workers";
        public static string Serap => "serap.workers";
        public static string SerapBoletimDeadLetter => "serap.boletim.workers.deadletter";
        public static string Logs => "EnterpriseApplicationLog";
        public static int SerapDeadLetterTtl => 10 * 60 * 1000; /*10 Min * 60 Seg * 1000 milisegundos = 10 minutos em milisegundos*/
        public static int SerapDeadLetterTtl_3 => 3 * 60 * 1000; /*10 Min * 60 Seg * 1000 milisegundos = 10 minutos em milisegundos*/
    }
}