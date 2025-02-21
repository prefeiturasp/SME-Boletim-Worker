using Nest;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Dados.Repositories
{
    public class RepositorioProva : RepositorioBase<Prova>, IRepositorioProva
    {
        private readonly ConnectionStringOptions connectionStrings;
        public RepositorioProva(ConnectionStringOptions connectionStrings):base(connectionStrings)
        {
            this.connectionStrings = connectionStrings ?? throw new ArgumentNullException(nameof(connectionStrings));
        }
    }
}