using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;

namespace SME.SERAp.Boletim.Dados.Mapeamentos
{
    public static class RegistrarMapeamentos
    {
        public static void Registrar()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new ProvaMap());
                config.AddMap(new BoletimProvaAlunoMap());
                config.AddMap(new AlunoProvaProficienciaMap());
                config.AddMap(new BoletimEscolarMap());
                config.AddMap(new BoletimLoteProvaMap());
                config.AddMap(new LoteProvaMap());
                config.AddMap(new BoletimResultadoProbabilidadeMap());
                config.AddMap(new NivelProficienciaMap());

                config.ForDommel();
            });
        }
    }
}
