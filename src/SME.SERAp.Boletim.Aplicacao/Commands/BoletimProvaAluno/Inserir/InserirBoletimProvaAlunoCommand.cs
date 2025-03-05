using MediatR;
using SME.SERAp.Boletim.Dominio.Entities;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class InserirBoletimProvaAlunoCommand : IRequest<long>
    {
        public InserirBoletimProvaAlunoCommand(BoletimProvaAluno boletimProvaAluno)
        {
            BoletimProvaAluno = boletimProvaAluno;
        }

        public BoletimProvaAluno BoletimProvaAluno { get; set; }
    }
}
