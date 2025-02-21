using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Policies
{
    public class PoliticaPolly
    {
        public static string PublicaFila => "RetryPolicyFilasRabbit";
    }
}