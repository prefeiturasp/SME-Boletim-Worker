using MediatR;
using Moq;
using RabbitMQ.Client;
using SME.SERAp.Boletim.Aplicacao.Queries;
using SME.SERAp.Boletim.Aplicacao.Queries.Elastic.ObterResumoGeralProvaPorUe;
using SME.SERAp.Boletim.Aplicacao.UseCases;
using SME.SERAp.Boletim.Dominio.Entities;
using SME.SERAp.Boletim.Infra.Dtos;
using SME.SERAp.Boletim.Infra.Dtos.Elastic;
using SME.SERAp.Boletim.Infra.Fila;
using SME.SERAp.Boletim.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SERAp.Boletim.Aplicacao.Testes.UseCases
{
    public class TratarProvasUesTotalAlunosAcompanhamentoUseCaseTeste
    {
        private readonly TratarProvasUesTotalAlunosAcompanhamentoUseCase useCase;
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IChannel> channelMock;
        private readonly Mock<IServicoLog> servicoLogMock;

        public TratarProvasUesTotalAlunosAcompanhamentoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            channelMock = new Mock<IChannel>();
            servicoLogMock = new Mock<IServicoLog>();

            useCase = new TratarProvasUesTotalAlunosAcompanhamentoUseCase(
                mediatorMock.Object,
                channelMock.Object,
                servicoLogMock.Object
            );
        }

        [Fact]
        public async Task Deve_Executar_Com_Sucesso()
        {
            var dto = ObterLoteUeDto();
            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(dto), Guid.NewGuid());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterResumoGeralProvaPorUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ResumoGeralProvaDto { TotalAlunos = 20 });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAlunosRealizaramProvaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BoletimLoteUeRealizaramProvaDto { RealizaramProva = 18 });

            mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirBoletimLoteUeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediatorMock.Setup(m => m.Send(It.IsAny<InserirBoletimLoteUeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Nao_Deve_Executar_Quando_Dto_For_Nulo()
        {
            var mensagem = new MensagemRabbit("null", Guid.NewGuid());

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Nao_Deve_Executar_Quando_ResumoGeral_Nulo()
        {
            var dto = ObterLoteUeDto();
            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(dto), Guid.NewGuid());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterResumoGeralProvaPorUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ResumoGeralProvaDto)null!);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Nao_Deve_Executar_Quando_TotalAlunos_Zero()
        {
            var dto = ObterLoteUeDto();
            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(dto), Guid.NewGuid());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterResumoGeralProvaPorUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ResumoGeralProvaDto { TotalAlunos = 0 });

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Deve_Atualizar_TotalAlunos_Quando_RealizaramProva_Maior()
        {
            var dto = ObterLoteUeDto();
            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(dto), Guid.NewGuid());

            var resumo = new ResumoGeralProvaDto { TotalAlunos = 15 };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterResumoGeralProvaPorUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resumo);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAlunosRealizaramProvaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BoletimLoteUeRealizaramProvaDto { RealizaramProva = 25 });

            mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirBoletimLoteUeCommand>(), It.IsAny<CancellationToken>()));
            mediatorMock.Setup(m => m.Send(It.IsAny<InserirBoletimLoteUeCommand>(), It.IsAny<CancellationToken>()));

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Nao_Deve_Atualizar_RealizaramProva_Se_For_Nulo()
        {
            var dto = ObterLoteUeDto();
            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(dto), Guid.NewGuid());

            var resumo = new ResumoGeralProvaDto { TotalAlunos = 20 };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterResumoGeralProvaPorUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resumo);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUesAlunosRealizaramProvaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BoletimLoteUeRealizaramProvaDto { RealizaramProva = 0 });

            mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirBoletimLoteUeCommand>(), It.IsAny<CancellationToken>()));
            mediatorMock.Setup(m => m.Send(It.IsAny<InserirBoletimLoteUeCommand>(), It.IsAny<CancellationToken>()));

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Deve_Registrar_Erro_Quando_Exception()
        {
            var dto = ObterLoteUeDto();
            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(dto), Guid.NewGuid());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterResumoGeralProvaPorUeQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro"));

            var resultado = await useCase.Executar(mensagem);

            Assert.False(resultado);
            servicoLogMock.Verify(s => s.Registrar(It.IsAny<Exception>()), Times.Once);
        }

        private LoteUeDto ObterLoteUeDto()
        {
            return new LoteUeDto
            {
                LoteId = 101,
                DreId = 201,
                UeId = 301,
                AnoEscolar = 2,
                ProvasIds = new List<long> { 1, 2 }
            };
        }
    }
}