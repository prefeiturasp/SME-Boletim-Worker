using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunosProvaProficienciaBoletimPorProvaId
{
    public class ObterAlunosProvaProficienciaBoletimPorProvaIdQueryHandler : IRequestHandler<ObterAlunosProvaProficienciaBoletimPorProvaIdQuery, IEnumerable<AlunoProvaProficienciaBoletimDto>>
    {
        private readonly IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia;

        public ObterAlunosProvaProficienciaBoletimPorProvaIdQueryHandler(IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia)
        {
            this.repositorioAlunoProvaProficiencia = repositorioAlunoProvaProficiencia;
        }

        public Task<IEnumerable<AlunoProvaProficienciaBoletimDto>> Handle(ObterAlunosProvaProficienciaBoletimPorProvaIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioAlunoProvaProficiencia.ObterAlunosProvaProficienciaBoletimPorProvaId(request.ProvaId);
        }
    }
}
