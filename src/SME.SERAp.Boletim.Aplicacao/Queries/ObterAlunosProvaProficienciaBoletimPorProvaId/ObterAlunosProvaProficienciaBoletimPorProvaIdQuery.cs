using MediatR;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunosProvaProficienciaBoletimPorProvaId
{
    public class ObterAlunosProvaProficienciaBoletimPorProvaIdQuery : IRequest<IEnumerable<AlunoProvaProficienciaBoletimDto>>
    {
        public ObterAlunosProvaProficienciaBoletimPorProvaIdQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
