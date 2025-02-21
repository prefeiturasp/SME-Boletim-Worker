using MediatR;
using SME.SERAp.Boletim.Dados.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Commands.Prova.Alterar
{
    public class AlterarProvaCommandHandler : IRequestHandler<AlterarProvaCommand, long>
    {
        private readonly IRepositorioProva repositorioProva;

        public AlterarProvaCommandHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }

        public async Task<long> Handle(AlterarProvaCommand request, CancellationToken cancellationToken)
        {
            return await repositorioProva.UpdateAsync(new Dominio.Entities.Prova(
                request.ProvaDto.Id,
                request.ProvaDto.Codigo,
                request.ProvaDto.Descricao,
                request.ProvaDto.Modalidade,
                request.ProvaDto.Inicio.Year,
                request.ProvaDto.Inicio,
                request.ProvaDto.Fim
                ));
        }
    }
}