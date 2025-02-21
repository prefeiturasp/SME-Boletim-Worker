﻿using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Dominio.Enums;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using SME.SERAp.Boletim.Infra.Extensions;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Services
{
    public class ServicoLog : IServicoLog
    {
        private readonly ILogger<ServicoLog> logger;
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly RabbitLogOptions configuracaoRabbitOptions;
        public ServicoLog(IServicoTelemetria servicoTelemetria, RabbitLogOptions configuracaoRabbitOptions, ILogger<ServicoLog> logger)
        {
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
            this.configuracaoRabbitOptions = configuracaoRabbitOptions ?? throw new System.ArgumentNullException(nameof(configuracaoRabbitOptions));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Registrar(Exception ex)
        {
            LogMensagem logMensagem = new LogMensagem("Exception --- ", LogNivel.Critico, ex.Message, ex.StackTrace);
            Registrar(logMensagem);
        }

        public void Registrar(LogNivel nivel, string erro, string observacoes, string stackTrace)
        {
            LogMensagem logMensagem = new LogMensagem(erro, nivel, observacoes, stackTrace);
            Registrar(logMensagem);

        }

        public void Registrar(string mensagem, Exception ex)
        {
            LogMensagem logMensagem = new LogMensagem(mensagem, LogNivel.Critico, ex.Message, ex.StackTrace);

            Registrar(logMensagem);
        }
        private void Registrar(LogMensagem log)
        {
            var body = Encoding.UTF8.GetBytes(log.ConverterObjectParaJson());
            servicoTelemetria.Registrar(() => PublicarMensagem(body), "RabbitMQ", "Salvar Log Via Rabbit", RotasRabbit.RotaLogs);
        }

        private async void PublicarMensagem(byte[] body)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = configuracaoRabbitOptions.HostName,
                    UserName = configuracaoRabbitOptions.UserName,
                    Password = configuracaoRabbitOptions.Password,
                    VirtualHost = configuracaoRabbitOptions.VirtualHost
                };

                using var conexaoRabbit = await factory.CreateConnectionAsync();
                using var channel = await conexaoRabbit.CreateChannelAsync();
                var props = new BasicProperties
                {
                    Persistent = true
                };

                await channel.BasicPublishAsync(
                    ExchangeRabbit.Logs,
                    RotasRabbit.RotaLogs,
                    true,
                    props,
                    body
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}