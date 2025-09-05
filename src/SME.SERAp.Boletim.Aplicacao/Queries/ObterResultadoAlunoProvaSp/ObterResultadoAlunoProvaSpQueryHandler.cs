using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterResultadoAlunoProvaSpQueryHandler : IRequestHandler<ObterResultadoAlunoProvaSpQuery, ResultadoAlunoProvaSpDto>
    {
        private readonly IRepositorioAlunoProvaSpProficiencia repositorio;
        public ObterResultadoAlunoProvaSpQueryHandler(IRepositorioAlunoProvaSpProficiencia repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<ResultadoAlunoProvaSpDto> Handle(ObterResultadoAlunoProvaSpQuery request, CancellationToken cancellationToken)
        {
           return repositorio.ObterResultadoAlunoProvaSp(request.Edicao, request.AreaDoConhecimento, request.AlunoMatricula);
        }
    }
}
