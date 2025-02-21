using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Fila
{
    public class RotasRabbit
    {
        public static string RotaLogs => "ApplicationLog";
        public static string Log => "ApplicationLog";

        public const string IniciarSync = "serap.boletim.iniciar.sync";

        public const string ProvaSync = "serap.boletim.prova.sync";
        public const string ProvaTratar = "serap.boletim.prova.tratar";
    }
}