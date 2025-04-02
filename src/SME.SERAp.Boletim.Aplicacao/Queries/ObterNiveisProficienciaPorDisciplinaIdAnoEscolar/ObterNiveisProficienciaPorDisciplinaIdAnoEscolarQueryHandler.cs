using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterNiveisProficienciaPorDisciplinaIdAnoEscolar
{
    public class ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQueryHandler
        : IRequestHandler<ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQuery, IEnumerable<NivelProficiencia>>
    {
        private readonly IRepositorioNivelProficiencia repositorioNivelProficiencia;
        public ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQueryHandler(IRepositorioNivelProficiencia repositorioNivelProficiencia)
        {
            this.repositorioNivelProficiencia = repositorioNivelProficiencia;
        }

        public Task<IEnumerable<NivelProficiencia>> Handle(ObterNiveisProficienciaPorDisciplinaIdAnoEscolarQuery request, CancellationToken cancellationToken)
        {
            return repositorioNivelProficiencia.ObterNiveisProficienciaPorDisciplinaIdAnoEscolar(request.DisciplinaId, request.AnoEscolar);
        }
    }
}
