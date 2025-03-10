using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Infra.Interfaces;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterQuantidadeMensagensPorNomeFila
{
    public class ObterQuantidadeMensagensPorNomeFilaQueryHandler
        : IRequestHandler<ObterQuantidadeMensagensPorNomeFilaQuery, int>
    {
        private readonly IServicoLog servicoLog;
        private readonly IConnection rabbitConnection;
        public ObterQuantidadeMensagensPorNomeFilaQueryHandler(IServicoLog servicoLog, IConnection rabbitConnection)
        {
            this.servicoLog = servicoLog;
            this.rabbitConnection = rabbitConnection;
        }

        public async Task<int> Handle(ObterQuantidadeMensagensPorNomeFilaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                using var channel = await rabbitConnection.CreateChannelAsync();
                var quantidadeMensagens = await channel.MessageCountAsync(request.NomeFila);
                return (int)quantidadeMensagens;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                throw;
            }
        }
    }
}
