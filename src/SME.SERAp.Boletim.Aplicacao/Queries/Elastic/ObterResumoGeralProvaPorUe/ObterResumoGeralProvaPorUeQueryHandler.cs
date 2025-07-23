using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces.Elastic;
using SME.SERAp.Boletim.Infra.Dtos.Elastic;

namespace SME.SERAp.Boletim.Aplicacao.Queries.Elastic.ObterResumoGeralProvaPorUe
{
    public class ObterResumoGeralProvaPorUeQueryHandler : IRequestHandler<ObterResumoGeralProvaPorUeQuery, ResumoGeralProvaDto>
    {
        private readonly IRepositorioElasticProvaTurmaResultado repositorioElasticProvaTurmaResultado;

        public ObterResumoGeralProvaPorUeQueryHandler(IRepositorioElasticProvaTurmaResultado repositorioProvaTurmaResultado)
        {
            this.repositorioElasticProvaTurmaResultado = repositorioProvaTurmaResultado ?? throw new ArgumentNullException(nameof(repositorioProvaTurmaResultado));
        }

        public async Task<ResumoGeralProvaDto> Handle(ObterResumoGeralProvaPorUeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioElasticProvaTurmaResultado.ObterResumoGeralPorUeAsync(request.UeId, request.ProvaId, request.AnoEscolar);
        }
    }
}