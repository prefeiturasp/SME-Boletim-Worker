using MediatR;

namespace SME.SERAp.Boletim.Aplicacao
{
    public class ExcluirBoletimProvaAlunoCommand : IRequest<int>
    {
        public ExcluirBoletimProvaAlunoCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
