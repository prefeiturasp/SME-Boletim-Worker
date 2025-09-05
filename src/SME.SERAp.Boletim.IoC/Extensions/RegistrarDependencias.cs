using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SERAp.Boletim.Aplicacao.Interfaces;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dados.Interfaces.Elastic;
using SME.SERAp.Boletim.Dados.Mapeamentos;
using SME.SERAp.Boletim.Dados.Repositories;
using SME.SERAp.Boletim.Dados.Repositories.Elastic;
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
            services.AddScoped<IRepositorioBoletimLoteProva, RepositorioBoletimLoteProva>();
            services.AddScoped<IRepositorioLoteProva, RepositorioLoteProva>();
            services.AddScoped<IRepositorioBoletimResultadoProbabilidade, RepositorioBoletimResultadoProbabilidade>();
            services.AddScoped<IRepositorioNivelProficiencia, RepositorioNivelProficiencia>();
            services.AddScoped<IRepositorioBoletimLoteUe, RepositorioBoletimLoteUe>();
            services.AddScoped<IRepositorioElasticProvaTurmaResultado, RepositorioElasticProvaTurmaResultado>();
            services.AddScoped<IRepositorioAlunoProvaSpProficiencia, RepositorioAlunoProvaSpProficiencia>();
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
            services.AddScoped<IBuscarProvasBoletimLoteUseCase, BuscarProvasBoletimLoteUseCase>();
            services.AddScoped<IBuscarBoletinsEscolaresProvaUseCase, BuscarBoletinsEscolaresProvaUseCase>();
            services.AddScoped<ITratarBoletimEscolarProvaUseCase, TratarBoletimEscolarProvaUseCase>();
            services.AddScoped<IBuscarTurmasBoletimResultadoProbabilidadeProvaUseCase, BuscarTurmasBoletimResultadoProbabilidadeProvaUseCase>();
            services.AddScoped<ITratarTurmaBoletimResultadoProbabilidadeProvaUseCase, TratarTurmaBoletimResultadoProbabilidadeProvaUseCase>();
            services.AddScoped<ITrataBoletimResultadoProbabilidadeProvaUseCase, TrataBoletimResultadoProbabilidadeProvaUseCase>();
            services.AddScoped<IBuscarAlunoProvaSpProficienciaUseCase, BuscarAlunoProvaSpProficienciaUseCase>();
            services.AddScoped<ITratarAlunoProvaSpProficienciaUseCase, TratarAlunoProvaSpProficienciaUseCase>();
            services.AddScoped<IBuscarProvaAlunosProvaSpProficienciaUseCase, BuscarProvaAlunosProvaSpProficienciaUseCase>();

            services.AddSingleton<IConsolidarBoletimEscolarLoteUseCase, ConsolidarBoletimEscolarLoteUseCase>();

            services.AddScoped<IBuscarBoletinsLotesUesUseCase, BuscarBoletinsLotesUesUseCase>();
            services.AddScoped<ITratarBoletimLoteUeUseCase, TratarBoletimLoteUeUseCase>();

            services.AddScoped<IBuscarProvasUesTotalAlunosAcompanhamentoUseCase, BuscarProvasUesTotalAlunosAcompanhamentoUseCase>();
            services.AddScoped<ITratarProvasUesTotalAlunosAcompanhamentoUseCase, TratarProvasUesTotalAlunosAcompanhamentoUseCase>();
        }
    }
}