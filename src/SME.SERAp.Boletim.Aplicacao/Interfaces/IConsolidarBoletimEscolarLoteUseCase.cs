namespace SME.SERAp.Boletim.Aplicacao.Interfaces
{
    public interface IConsolidarBoletimEscolarLoteUseCase
    {
        Task Executar(long loteId);
    }
}
