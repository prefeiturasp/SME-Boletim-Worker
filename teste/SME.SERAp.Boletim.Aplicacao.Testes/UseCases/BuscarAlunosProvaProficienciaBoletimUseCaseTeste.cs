using MediatR;
using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Commands.PublicaFilaRabbit;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterAlunosProvaProficienciaBoletimPorProvaId;
using SME.SERAp.Boletim.Aplicacao.Queries.ObterProvaPorId;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;
using System.Text.Json;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UsesCases
{
    public class BuscarAlunosProvaProficienciaBoletimUseCaseTeste
    {
        private readonly BuscarAlunosProvaProficienciaBoletimUseCase buscarAlunosProvaProficienciaBoletimUseCase;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IChannel> channel;
        private readonly Mock<IServicoLog> serviceLog;

        public BuscarAlunosProvaProficienciaBoletimUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            channel = new Mock<IChannel>();
            serviceLog = new Mock<IServicoLog>();
            buscarAlunosProvaProficienciaBoletimUseCase = new BuscarAlunosProvaProficienciaBoletimUseCase(mediator.Object, channel.Object, serviceLog.Object);
        }

        [Fact]
        public async Task Deve_Buscar_Alunos_Prova_Proficiencia_Boletim()
        {
            var provaFinalizadaDto = ObterProvaFinalizadaDto();

            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(provaFinalizadaDto), Guid.NewGuid());
            var alunosProvasProficienciaBoletimDtos = ObterAlunosProvasProficienciaBoletimDtos();
            mediator.Setup(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Prova { Id = 1 });

            mediator.Setup(m => m.Send(It.IsAny<ObterAlunosProvaProficienciaBoletimPorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosProvasProficienciaBoletimDtos);

            var resultado = await buscarAlunosProvaProficienciaBoletimUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(alunosProvasProficienciaBoletimDtos.Count));
        }

        [Fact]
        public async Task Nao_Deve_Publicar_Tratar_Boletim_ProvaAluno()
        {
            var provaFinalizadaDto = ObterProvaFinalizadaDto();

            var mensagemRabbit = new MensagemRabbit(JsonSerializer.Serialize(provaFinalizadaDto), Guid.NewGuid());

            mediator.Setup(m => m.Send(It.IsAny<ObterProvaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Prova { Id = 1 });

            mediator.Setup(m => m.Send(It.IsAny<ObterAlunosProvaProficienciaBoletimPorProvaIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<AlunoProvaProficienciaBoletimDto>>(null!));

            var resultado = await buscarAlunosProvaProficienciaBoletimUseCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.IsAny<PublicaFilaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private static ProvaDto ObterProvaFinalizadaDto()
        {
            return new ProvaDto
            {
                Id = 1,
                LoteId = 123,
            };
        }

        private static List<AlunoProvaProficienciaBoletimDto> ObterAlunosProvasProficienciaBoletimDtos()
        {
            return new List<AlunoProvaProficienciaBoletimDto>
            {
                new AlunoProvaProficienciaBoletimDto { CodigoAluno = 1, Proficiencia = 200, BoletimLoteId = 123 },
                new AlunoProvaProficienciaBoletimDto { CodigoAluno = 2, Proficiencia = 250, BoletimLoteId = 123 },
            };
        }
    }
}
