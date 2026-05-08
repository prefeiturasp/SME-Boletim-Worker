using SME.SERAp.Boletim.Infra.Dtos;

namespace SME.SERAp.Boletim.Infra.Testes.Dtos
{
    public class BoletimProvaAlunoUltimaTurmaAlunoDtoTeste
    {
        [Fact]
        public void Deve_Criar_Instance_Com_Parametros_Padrao()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();

            Assert.NotNull(dto);
            Assert.Equal(0, dto.CodigoDre);
            Assert.Null(dto.CodigoUe);
            Assert.Null(dto.NomeUe);
            Assert.Equal(0, dto.AnoEscolar);
            Assert.Null(dto.Turma);
            Assert.Equal(0, dto.NivelCodigo);
        }

        [Fact]
        public void Deve_Atribuir_CodigoDre()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();
            var codigoDreEsperado = 123L;

            dto.CodigoDre = codigoDreEsperado;

            Assert.Equal(codigoDreEsperado, dto.CodigoDre);
        }

        [Fact]
        public void Deve_Atribuir_CodigoUe()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();
            var codigoUeEsperado = "UE001";

            dto.CodigoUe = codigoUeEsperado;

            Assert.Equal(codigoUeEsperado, dto.CodigoUe);
        }

        [Fact]
        public void Deve_Atribuir_NomeUe()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();
            var nomeUeEsperado = "Escola A";

            dto.NomeUe = nomeUeEsperado;

            Assert.Equal(nomeUeEsperado, dto.NomeUe);
        }

        [Fact]
        public void Deve_Atribuir_AnoEscolar()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();
            var anoEscolarEsperado = 5;

            dto.AnoEscolar = anoEscolarEsperado;

            Assert.Equal(anoEscolarEsperado, dto.AnoEscolar);
        }

        [Fact]
        public void Deve_Atribuir_Turma()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();
            var turmaEsperada = "5A";

            dto.Turma = turmaEsperada;

            Assert.Equal(turmaEsperada, dto.Turma);
        }

        [Fact]
        public void Deve_Atribuir_NivelCodigo()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();
            var nivelCodigoEsperado = 3;

            dto.NivelCodigo = nivelCodigoEsperado;

            Assert.Equal(nivelCodigoEsperado, dto.NivelCodigo);
        }

        [Fact]
        public void Deve_Atribuir_Todas_As_Propriedades()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 111L,
                CodigoUe = "UE001",
                NomeUe = "Escola A",
                AnoEscolar = 5,
                Turma = "5A",
                NivelCodigo = 3
            };

            Assert.Equal(111L, dto.CodigoDre);
            Assert.Equal("UE001", dto.CodigoUe);
            Assert.Equal("Escola A", dto.NomeUe);
            Assert.Equal(5, dto.AnoEscolar);
            Assert.Equal("5A", dto.Turma);
            Assert.Equal(3, dto.NivelCodigo);
        }

        [Fact]
        public void Deve_Modificar_Propriedade_Multiplas_Vezes()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();

            dto.CodigoDre = 100L;
            Assert.Equal(100L, dto.CodigoDre);

            dto.CodigoDre = 200L;
            Assert.Equal(200L, dto.CodigoDre);

            dto.CodigoDre = 300L;
            Assert.Equal(300L, dto.CodigoDre);
        }

        [Fact]
        public void Deve_Lidar_Com_Valores_Nulos()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoUe = null,
                NomeUe = null,
                Turma = null
            };

            Assert.Null(dto.CodigoUe);
            Assert.Null(dto.NomeUe);
            Assert.Null(dto.Turma);
        }

        [Fact]
        public void Deve_Lidar_Com_Strings_Vazias()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoUe = string.Empty,
                NomeUe = string.Empty,
                Turma = string.Empty
            };

            Assert.Empty(dto.CodigoUe);
            Assert.Empty(dto.NomeUe);
            Assert.Empty(dto.Turma);
        }

        [Fact]
        public void Deve_Lidar_Com_Valores_Numericos_Extremos()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = long.MaxValue,
                AnoEscolar = int.MaxValue,
                NivelCodigo = int.MaxValue
            };

            Assert.Equal(long.MaxValue, dto.CodigoDre);
            Assert.Equal(int.MaxValue, dto.AnoEscolar);
            Assert.Equal(int.MaxValue, dto.NivelCodigo);
        }

        [Fact]
        public void Deve_Lidar_Com_Valores_Numericos_Negativos()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = -100L,
                AnoEscolar = -5,
                NivelCodigo = -1
            };

            Assert.Equal(-100L, dto.CodigoDre);
            Assert.Equal(-5, dto.AnoEscolar);
            Assert.Equal(-1, dto.NivelCodigo);
        }

        [Fact]
        public void Deve_Lidar_Com_Valores_Numericos_Zero()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 0,
                AnoEscolar = 0,
                NivelCodigo = 0
            };

            Assert.Equal(0L, dto.CodigoDre);
            Assert.Equal(0, dto.AnoEscolar);
            Assert.Equal(0, dto.NivelCodigo);
        }

        [Fact]
        public void Deve_Criar_Multiplas_Instancias_Independentes()
        {
            var dto1 = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 111L,
                AnoEscolar = 5,
                Turma = "5A"
            };

            var dto2 = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 222L,
                AnoEscolar = 6,
                Turma = "6B"
            };

            Assert.NotEqual(dto1.CodigoDre, dto2.CodigoDre);
            Assert.NotEqual(dto1.AnoEscolar, dto2.AnoEscolar);
            Assert.NotEqual(dto1.Turma, dto2.Turma);
        }

        [Fact]
        public void Deve_Modificar_Uma_Instancia_Sem_Afetar_Outra()
        {
            var dto1 = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 100L,
                NivelCodigo = 2
            };

            var dto2 = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 200L,
                NivelCodigo = 3
            };

            dto1.CodigoDre = 500L;

            Assert.Equal(500L, dto1.CodigoDre);
            Assert.Equal(200L, dto2.CodigoDre);
        }

        [Fact]
        public void Deve_Suportar_Strings_Longas()
        {
            var codigoUeLongo = new string('A', 1000);
            var nomeUeLongo = new string('B', 1000);
            var turmaLonga = new string('C', 1000);

            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoUe = codigoUeLongo,
                NomeUe = nomeUeLongo,
                Turma = turmaLonga
            };

            Assert.Equal(codigoUeLongo, dto.CodigoUe);
            Assert.Equal(nomeUeLongo, dto.NomeUe);
            Assert.Equal(turmaLonga, dto.Turma);
        }

        [Fact]
        public void Deve_Suportar_Caracteres_Especiais_Em_Strings()
        {
            var codigoUeEspecial = "UE@#$%";
            var nomeUeEspecial = "Escola™ © ®";
            var turmaEspecial = "5ª-A";

            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoUe = codigoUeEspecial,
                NomeUe = nomeUeEspecial,
                Turma = turmaEspecial
            };

            Assert.Equal(codigoUeEspecial, dto.CodigoUe);
            Assert.Equal(nomeUeEspecial, dto.NomeUe);
            Assert.Equal(turmaEspecial, dto.Turma);
        }

        [Fact]
        public void Deve_Validar_Tipo_De_Propriedade_CodigoDre()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();
            dto.CodigoDre = 123L;

            Assert.IsType<long>(dto.CodigoDre);
        }

        [Fact]
        public void Deve_Validar_Tipo_De_Propriedade_CodigoUe()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();
            dto.CodigoUe = "UE001";

            Assert.IsType<string>(dto.CodigoUe);
        }

        [Fact]
        public void Deve_Validar_Tipo_De_Propriedade_NomeUe()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();
            dto.NomeUe = "Escola A";

            Assert.IsType<string>(dto.NomeUe);
        }

        [Fact]
        public void Deve_Validar_Tipo_De_Propriedade_AnoEscolar()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();
            dto.AnoEscolar = 5;

            Assert.IsType<int>(dto.AnoEscolar);
        }

        [Fact]
        public void Deve_Validar_Tipo_De_Propriedade_Turma()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();
            dto.Turma = "5A";

            Assert.IsType<string>(dto.Turma);
        }

        [Fact]
        public void Deve_Validar_Tipo_De_Propriedade_NivelCodigo()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();
            dto.NivelCodigo = 3;

            Assert.IsType<int>(dto.NivelCodigo);
        }

        [Fact]
        public void Deve_Permitir_Alterar_CodigoDre_De_Zero_Para_Valor()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();
            Assert.Equal(0L, dto.CodigoDre);

            dto.CodigoDre = 999L;
            Assert.Equal(999L, dto.CodigoDre);
        }

        [Fact]
        public void Deve_Permitir_Alterar_AnoEscolar_De_Zero_Para_Valor()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();
            Assert.Equal(0, dto.AnoEscolar);

            dto.AnoEscolar = 9;
            Assert.Equal(9, dto.AnoEscolar);
        }

        [Fact]
        public void Deve_Permitir_Alterar_NivelCodigo_De_Zero_Para_Valor()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto();
            Assert.Equal(0, dto.NivelCodigo);

            dto.NivelCodigo = 4;
            Assert.Equal(4, dto.NivelCodigo);
        }

        [Fact]
        public void Deve_Permitir_Copia_De_Propriedades_Entre_Instancias()
        {
            var dto1 = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 111L,
                CodigoUe = "UE001",
                NomeUe = "Escola A",
                AnoEscolar = 5,
                Turma = "5A",
                NivelCodigo = 3
            };

            var dto2 = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = dto1.CodigoDre,
                CodigoUe = dto1.CodigoUe,
                NomeUe = dto1.NomeUe,
                AnoEscolar = dto1.AnoEscolar,
                Turma = dto1.Turma,
                NivelCodigo = dto1.NivelCodigo
            };

            Assert.Equal(dto1.CodigoDre, dto2.CodigoDre);
            Assert.Equal(dto1.CodigoUe, dto2.CodigoUe);
            Assert.Equal(dto1.NomeUe, dto2.NomeUe);
            Assert.Equal(dto1.AnoEscolar, dto2.AnoEscolar);
            Assert.Equal(dto1.Turma, dto2.Turma);
            Assert.Equal(dto1.NivelCodigo, dto2.NivelCodigo);
        }

        [Fact]
        public void Deve_Validar_Todas_Propriedades_Com_Dados_Reais()
        {
            var dto = new BoletimProvaAlunoUltimaTurmaAlunoDto
            {
                CodigoDre = 1234L,
                CodigoUe = "1234",
                NomeUe = "EMEF Exemplo",
                AnoEscolar = 5,
                Turma = "5A",
                NivelCodigo = 2
            };

            Assert.True(dto.CodigoDre > 0);
            Assert.False(string.IsNullOrEmpty(dto.CodigoUe));
            Assert.False(string.IsNullOrEmpty(dto.NomeUe));
            Assert.True(dto.AnoEscolar > 0);
            Assert.False(string.IsNullOrEmpty(dto.Turma));
            Assert.True(dto.NivelCodigo > 0);
        }
    }
}
