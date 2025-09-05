using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos
{
    public class BoletimResultadoProbabilidadeDtoTeste
    {
        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente_Com_Construtor_Padrao()
        {
            var dto = new BoletimResultadoProbabilidadeDto
            {
                HabilidadeId = 1,
                CodigoHabilidade = "HAB001",
                HabilidadeDescricao = "Descrição da habilidade",
                TurmaDescricao = "Turma A",
                TurmaId = 10,
                ProvaId = 20,
                UeId = 30,
                DisciplinaId = 40,
                AnoEscolar = 5,
                AbaixoDoBasico = 0.1,
                Basico = 0.2,
                Adequado = 0.3,
                Avancado = 0.4
            };

            Assert.Equal(1, dto.HabilidadeId);
            Assert.Equal("HAB001", dto.CodigoHabilidade);
            Assert.Equal("Descrição da habilidade", dto.HabilidadeDescricao);
            Assert.Equal("Turma A", dto.TurmaDescricao);
            Assert.Equal(10, dto.TurmaId);
            Assert.Equal(20, dto.ProvaId);
            Assert.Equal(30, dto.UeId);
            Assert.Equal(40, dto.DisciplinaId);
            Assert.Equal(5, dto.AnoEscolar);
            Assert.Equal(0.1, dto.AbaixoDoBasico);
            Assert.Equal(0.2, dto.Basico);
            Assert.Equal(0.3, dto.Adequado);
            Assert.Equal(0.4, dto.Avancado);
        }

        [Fact]
        public void Deve_Atribuir_Propriedades_Corretamente_Com_Construtor_Parametrizado()
        {
            var dto = new BoletimResultadoProbabilidadeDto(
                habilidadeId: 2,
                codigoHabilidade: "HAB002",
                habilidadeDescricao: "Outra habilidade",
                turmaDescricao: "Turma B",
                turmaId: 11,
                provaId: 21,
                ueId: 31,
                disciplinaId: 41,
                anoEscolar: 6,
                abaixoDoBasico: 0.15,
                basico: 0.25,
                adequado: 0.35,
                avancado: 0.45
            );

            Assert.Equal(2, dto.HabilidadeId);
            Assert.Equal("HAB002", dto.CodigoHabilidade);
            Assert.Equal("Outra habilidade", dto.HabilidadeDescricao);
            Assert.Equal("Turma B", dto.TurmaDescricao);
            Assert.Equal(11, dto.TurmaId);
            Assert.Equal(21, dto.ProvaId);
            Assert.Equal(31, dto.UeId);
            Assert.Equal(41, dto.DisciplinaId);
            Assert.Equal(6, dto.AnoEscolar);
            Assert.Equal(0.15, dto.AbaixoDoBasico);
            Assert.Equal(0.25, dto.Basico);
            Assert.Equal(0.35, dto.Adequado);
            Assert.Equal(0.45, dto.Avancado);
        }
    }
}
