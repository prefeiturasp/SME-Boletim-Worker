using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterLotesProvaPorData;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvasFinalizadasPorData;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Extensions;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;
using System.Globalization;

namespace SME.SERAp.Boletim.Aplicacao.UseCases
{
    public class BuscarProvasFinalizadasUseCase : AbstractUseCase, IBuscarProvasFinalizadasUseCase
    {
        private readonly IServicoLog servicoLog;
        public BuscarProvasFinalizadasUseCase(IMediator mediator, IChannel channel, IServicoLog servicoLog) : base(mediator, channel)
        {
            this.servicoLog = servicoLog;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var dataProvasFinalizadas = DateTime.Now.AddDays(-1).Date;
                var provasFinalizadas = await mediator.Send(new ObterProvasFinalizadasPorDataQuery(dataProvasFinalizadas));
                if (provasFinalizadas?.Any() ?? false)
                {
                    await DesativarTodosLotesProvaAtivos();

                    await CriarLotesProva(provasFinalizadas);

                    foreach (var provaFinalizada in provasFinalizadas)
                    {
                        var boletimLoteProva = ObterBoletimLoteProva(provaFinalizada);
                        await mediator.Send(new InserirBoletimLoteProvaCommand(boletimLoteProva));

                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.BuscarAlunosProvaProficienciaBoletim, provaFinalizada));
                    }
                }
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                return false;
            }

            return true;
        }

        private async Task CriarLotesProva(IEnumerable<ProvaDto> provasFinalizadas)
        {
            var provasTai = provasFinalizadas.Where(x => x.FormatoTai);
            var provasNaoTai = provasFinalizadas.Where(x => !x.FormatoTai);

            if (provasTai?.Any() ?? false)
            {
                var provaBase = provasTai.OrderBy(x => x.Inicio).FirstOrDefault();
                var loteProva = await ObterLoteProva(provaBase, true);
                var loteId = await mediator.Send(new InserirLoteProvaCommand(loteProva));

                foreach (var prova in provasTai)
                    prova.LoteId = loteId;
            }

            if (provasNaoTai?.Any() ?? false)
            {
                var provaBase = provasNaoTai.OrderBy(x => x.Inicio).FirstOrDefault();
                var loteProva = await ObterLoteProva(provaBase, !provasTai.Any());
                var loteId = await mediator.Send(new InserirLoteProvaCommand(loteProva));

                foreach (var prova in provasNaoTai)
                    prova.LoteId = loteId;
            }
        }

        private async Task<LoteProva> ObterLoteProva(ProvaDto provaBase, bool exibirNoBoletim)
        {
            var nomeLote = await ObterNomeLote(provaBase);
            return new LoteProva(nomeLote, provaBase.FormatoTai, exibirNoBoletim,
                provaBase.DataCorrecaoFim, provaBase.Inicio);
        }

        private async Task<string> ObterNomeLote(ProvaDto provaBase)
        {
            var prefixoNomeLote = provaBase.FormatoTai ?
                "Saberes e aprendizagens ({0} {1})" :
                "Aplicação de prova ({0} {1})";

            var dataProva = provaBase.Inicio;
            var mesProva = dataProva.ToString("MMMM", new CultureInfo("pt-BR"));
            var anoProva = dataProva.ToString("yyyy");

            var nomeLote = string.Format(prefixoNomeLote, mesProva, anoProva);
            var aplicacoesMesProvaBase = await ObterAplicacoesMesProvaBase(provaBase);
            if (aplicacoesMesProvaBase?.Any() ?? false)
            {
                var aplicacaoProvaBase = aplicacoesMesProvaBase.Count() + 1;
                nomeLote += $" - {aplicacaoProvaBase}ª aplicação";
            }

            return nomeLote;
        }

        private async Task<IEnumerable<LoteProva>> ObterAplicacoesMesProvaBase(ProvaDto provaBase)
        {
            var inicio = provaBase.Inicio.InicioMes();
            var fim = provaBase.Inicio.FinalMes();

            return await mediator
                .Send(new ObterLotesProvaPorDataQuery(inicio, fim, provaBase.FormatoTai));
        }

        private static BoletimLoteProva ObterBoletimLoteProva(ProvaDto provaFinalizada)
        {
            return new BoletimLoteProva(provaFinalizada.Id, provaFinalizada.LoteId);
        }

        private async Task DesativarTodosLotesProvaAtivos()
        {
            await mediator.Send(new DesativarTodosLotesProvaAtivosCommand());
        }
    }
}
