using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao.Commands
{
    public class InserirAlunoProvaSpProficienciaCommand : IRequest<long>
    {
        public AlunoProvaSpProficiencia AlunoProvaSpProficiencia { get; set; }
        public InserirAlunoProvaSpProficienciaCommand(AlunoProvaSpProficiencia alunoProvaSpProficiencia)
        {
            AlunoProvaSpProficiencia = alunoProvaSpProficiencia;
        }
    }
}
