using Elastic.Apm.AspNetCore;
using Elastic.Apm.Config;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.Instrumentations.SqlClient;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Dados.Interceptors;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using SME.SERAp.Boletim.Infra.Interfaces;
using SME.SERAp.Boletim.Infra.Services;
using SME.SERAp.Boletim.Worker;
using SME.SERAp.Boletim.Dados;
using SME.SERAp.Boletim.Infra;
using SME.SERAp.Boletim.Infra.EnvironmentVariables;
using SME.SERAp.Boletim.IoC;
using System;
using System.Threading;
using SME.SERAp.Boletim.IoC.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using System.Reflection;

namespace SME.SERAp.Boletim.Worker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(a => a.AddUserSecrets(Assembly.GetExecutingAssembly()))
           .UseStartup<Startup>();
    }
}