using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.EnvironmentVariables
{
    public class ConnectionStringOptions
    {
        public static string Secao => "ConnectionStrings";
        public string ApiSerapExterna { get; set; }
        public string ApiSerap { get; set; }
        public string ApiSerapLeitura { get; set; }
        public string ApiSgp { get; set; }
        public string Eol { get; set; }
        public string CoreSSO { get; set; }
        public string ProvaSP { get; set; }
    }
}