using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterUesAlunosRealizaramProvaQueryHandler : IRequestHandler<ObterUesAlunosRealizaramProvaQuery, BoletimLoteUeRealizaramProvaDto>
    {
        private readonly IRepositorioBoletimLoteUe repositorioBoletimLoteUe;
        public ObterUesAlunosRealizaramProvaQueryHandler(IRepositorioBoletimLoteUe repositorioBoletimLoteUe)
        {
            this.repositorioBoletimLoteUe = repositorioBoletimLoteUe;
        }

        public Task<BoletimLoteUeRealizaramProvaDto> Handle(ObterUesAlunosRealizaramProvaQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioBoletimLoteUe.ObterUesAlunosRealizaramProva(request.LoteId, request.UeId, request.AnoEscolar);
        }
    }
}
