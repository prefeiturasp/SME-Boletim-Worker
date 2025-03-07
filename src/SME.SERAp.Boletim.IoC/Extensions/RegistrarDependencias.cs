using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dados.Mapeamentos;
using SME.SERAp.Boletim.Dados.Repositories;
using SME.SERAp.Boletim.Infra.Interfaces;
using SME.SERAp.Boletim.Infra.Services;

namespace SME.SERAp.Boletim.IoC.Extensions
{
    public static class RegistraDependencias
    {
        public static void Registrar(IServiceCollection services)
        {
            services.AdicionarMediatr();
            services.AdicionarValidadoresFluentValidation();
            services.AddPoliticas();

            RegistrarServicos(services);
            RegistrarRepositorios(services);
            RegistrarRepositoriosCoresso(services);
            RegistrarRepositoriosEol(services);
            RegistrarCasosDeUso(services);
            RegistrarMapeamentos.Registrar();
        }

        private static void RegistrarServicos(IServiceCollection services)
        {
            services.TryAddSingleton<IServicoLog, ServicoLog>();
            services.TryAddSingleton<IServicoMensageria, ServicoMensageria>();
        }

        private static void RegistrarRepositorios(IServiceCollection services)
        {
            services.AddScoped<IRepositorioProva, RepositorioProva>();
            services.AddScoped<IRepositorioAlunoProvaProficiencia, RepositorioAlunoProvaProficiencia>();
            services.AddScoped<IRepositorioBoletimProvaAluno, RepositorioBoletimProvaAluno>();
            services.AddScoped<IRepositorioBoletimEscolar, RepositorioBoletimEscolar>();
        }

        private static void RegistrarRepositoriosEol(IServiceCollection services)
        {

        }

        private static void RegistrarRepositoriosCoresso(IServiceCollection services)
        {

        }

        private static void RegistrarCasosDeUso(IServiceCollection services)
        {
            services.AddScoped<IBuscarProvasFinalizadasUseCase, BuscarProvasFinalizadasUseCase>();
            services.AddScoped<IBuscarAlunosProvaProficienciaBoletimUseCase, BuscarAlunosProvaProficienciaBoletimUseCase>();
            services.AddScoped<ITratarBoletimProvaAlunoUseCase, TratarBoletimProvaAlunoUseCase>();
            services.AddScoped<IBuscarBoletinsEscolaresProvaUseCase, BuscarBoletinsEscolaresProvaUseCase>();
            services.AddScoped<ITratarBoletimEscolarProvaUseCase, TratarBoletimEscolarProvaUseCase>();
        }
    }
}