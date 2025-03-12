using MediatR;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterQuantidadeMensagensPorNomeFila
{
    public class ObterQuantidadeMensagensPorNomeFilaQuery : IRequest<int>
    {
        public ObterQuantidadeMensagensPorNomeFilaQuery(string nomeFila)
        {
            NomeFila = nomeFila;
        }

        public string NomeFila { get; set; }
    }
}
