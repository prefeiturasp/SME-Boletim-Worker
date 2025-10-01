using MediatR;

namespace SME.SERAp.Boletim.Aplicacao.Commands.PublicarFilaRabbitSerapEstudante
{
    public class PublicarFilaRabbitSerapEstudanteCommand : IRequest<bool>
    {
        public string NomeFila { get; private set; }
        public string NomeRota { get; private set; }
        public object Mensagem { get; private set; }

        public PublicarFilaRabbitSerapEstudanteCommand(string nomeFila, object mensagem = null)
        {
            Mensagem = mensagem;
            NomeFila = nomeFila;
            NomeRota = nomeFila;
        }
    }
}
