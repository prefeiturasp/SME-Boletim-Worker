using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SME.SERAp.Boletim.Infra.Services.ServicoTelemetria;

namespace SME.SERAp.Boletim.Infra.Interfaces
{
    public interface IServicoTelemetria
    {
        Task<dynamic> RegistrarComRetornoAsync<T>(Func<Task<object>> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros);
        Task<dynamic> RegistrarComRetornoAsync<T>(Func<Task<object>> acao, string acaoNome, string telemetriaNome, string telemetriaValor);
        dynamic RegistrarComRetorno<T>(Func<object> acao, string acaoNome, string telemetriaNome, string telemetriaValor);
        void Registrar(Action acao, string acaoNome, string telemetriaNome, string telemetriaValor);
        Task RegistrarAsync(Func<Task> acao, string acaoNome, string telemetriaNome, string telemetriaValor);
        ServicoTelemetriaTransacao IniciarTransacao(string rota);
        void FinalizarTransacao(ServicoTelemetriaTransacao servicoTelemetriaTransacao);
        void RegistrarExcecao(ServicoTelemetriaTransacao servicoTelemetriaTransacao, Exception ex);
    }
}