using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.Instrumentations.SqlClient;
using Elasticsearch.Net;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Dados.Interceptors;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using SME.SERAp.Boletim.Infra.Interfaces;
using SME.SERAp.Boletim.Infra.Services;
using SME.SERAp.Boletim.IoC.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MongoDB.Driver.WriteConcern;

namespace SME.SERAp.Boletim.Worker
{
    internal class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigEnvoiromentVariables(services);
            RegistraDependencias.Registrar(services);

            services.AddHostedService<WorkerRabbit>();
        }

        private void ConfigEnvoiromentVariables(IServiceCollection services)
        {
            ConfigurarConexoes(services);
            ConfigurarRabbitmq(services);
            ConfigurarRabbitmqLog(services);
            ConfigurarTelemetria(services);
            ConfigurarCoresso(services);
            ConfigurarEol(services);
        }

        private void ConfigurarEol(IServiceCollection services)
        {
            var eolOptions = new EolOptions();
            Configuration.GetSection(EolOptions.Secao).Bind(eolOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(eolOptions);
        }

        private void ConfigurarCoresso(IServiceCollection services)
        {
            var coressoOptions = new CoressoOptions();
            Configuration.GetSection(CoressoOptions.Secao).Bind(coressoOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(coressoOptions);
        }

        private void ConfigurarConexoes(IServiceCollection services)
        {
            var connectionStringOptions = new ConnectionStringOptions();
            Configuration.GetSection(ConnectionStringOptions.Secao).Bind(connectionStringOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(connectionStringOptions);
        }

        private void ConfigurarTelemetria(IServiceCollection services)
        {
            var telemetriaOptions = new TelemetriaOptions();
            Configuration.GetSection(TelemetriaOptions.Secao).Bind(telemetriaOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(telemetriaOptions);

            var servicoTelemetria = new ServicoTelemetria(telemetriaOptions);
            services.AddSingleton<IServicoTelemetria>(servicoTelemetria);
            DapperExtensionMethods.Init(servicoTelemetria);
        }

        private void ConfigurarRabbitmqLog(IServiceCollection services)
        {
            var rabbitLogOptions = new RabbitLogOptions();
            Configuration.GetSection(RabbitLogOptions.Secao).Bind(rabbitLogOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(rabbitLogOptions);

            var factoryLog = new ConnectionFactory
            {
                HostName = rabbitLogOptions.HostName,
                UserName = rabbitLogOptions.UserName,
                Password = rabbitLogOptions.Password,
                VirtualHost = rabbitLogOptions.VirtualHost
            };

            var conexaoRabbitLog = factoryLog.CreateConnectionAsync().Result;
            IChannel channelLog = conexaoRabbitLog.CreateChannelAsync().Result;
        }

        private void ConfigurarRabbitmq(IServiceCollection services)
        {
            var rabbitOptions = new RabbitOptions();
            Configuration.GetSection(RabbitOptions.Secao).Bind(rabbitOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(rabbitOptions);

            var factory = new ConnectionFactory
            {
                HostName = rabbitOptions.HostName,
                UserName = rabbitOptions.UserName,
                Password = rabbitOptions.Password,
                VirtualHost = rabbitOptions.VirtualHost
            };

            services.AddSingleton(factory);

            services.AddSingleton<RabbitMQ.Client.IConnection>(provider =>
            {
                var factory = provider.GetRequiredService<ConnectionFactory>();
                return factory.CreateConnectionAsync().Result;
            });

            services.AddSingleton<IChannel>(provider =>
            {
                var connection = provider.GetRequiredService<RabbitMQ.Client.IConnection>();
                return connection.CreateChannelAsync().Result;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("workerrabbitmq!");
            });
        }
    }
}