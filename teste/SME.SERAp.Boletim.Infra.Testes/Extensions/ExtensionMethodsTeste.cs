using SME.SERAp.Boletim.Infra.Extensions;
using System.Reflection;

namespace SME.SERAp.Boletim.Infra.Testes.Extensions
{
    public class ExtensionMethodsTeste
    {
        private class ClasseTeste
        {
            public const int ConstanteInt = 42;
            public const string ConstanteString = "Teste";
            public static string OutraConstanteString = "Teste 2";

            public string MetodoSimples() => "Ok";

            public async Task<string> MetodoAsync()
            {
                await Task.Delay(10);
                return "AsyncOk";
            }
        }

        private interface ITesteInterface
        {
            void MetodoDaInterface();
        }

        private class ClasseComInterface : ITesteInterface
        {
            public void MetodoDaInterface() { }
        }

        private class ClasseHeranca : ClasseComInterface
        {
        }

        private class ClasseSemMetodo
        {
            // Nenhum método aqui
        }

        private interface IBase
        {
            void MetodoBase();
        }

        private interface IFilha : IBase
        {
        }

        private class ClasseInterfaceFilha : IFilha
        {
            public void MetodoBase() { }
        }

        private abstract class BaseAbstrata
        {
            public void MetodoAbstrato() { }
        }

        private class ClasseDerivada : BaseAbstrata { }

        private interface IInterfaceNaoImplementada
        {
            void MetodoSomenteInterface();
        }

        private abstract class ClasseAbstrataInterface : IInterfaceNaoImplementada
        {
            public abstract void MetodoSomenteInterface();
        }

        private interface IInterface1 { }
        private interface IInterface2 { void MetodoInterface2(); }

        private class ClasseComMultiplasInterfaces : IInterface1, IInterface2
        {
            public void MetodoInterface2() { }
        }

        [Fact]
        public void Deve_Obter_ConstantesPublicas_Corretamente()
        {
            var constantesInt = typeof(ClasseTeste).ObterConstantesPublicas<int>();
            var constantesString = typeof(ClasseTeste).ObterConstantesPublicas<string>();

            Assert.Single(constantesInt);
            Assert.Equal(42, constantesInt.First());

            Assert.Single(constantesString);
            Assert.Equal("Teste", constantesString.First());
        }

        [Fact]
        public void Nao_Deve_Obter_Static_NaoConst()
        {
            var constantes = typeof(ClasseTeste).ObterConstantesPublicas<string>();

            // Certifique-se de que "OutraConstanteString" não está presente
            Assert.DoesNotContain("Teste 2", constantes);
        }

        [Fact]
        public void Deve_Obter_Metodo_Corretamente()
        {
            var metodo = typeof(ClasseTeste).ObterMetodo("MetodoSimples");

            Assert.NotNull(metodo);
            Assert.Equal("MetodoSimples", metodo.Name);
        }

        [Fact]
        public async Task Deve_InvokeAsync_Corretamente()
        {
            var instancia = new ClasseTeste();
            var metodo = typeof(ClasseTeste).ObterMetodo("MetodoAsync");

            var resultado = await metodo.InvokeAsync(instancia);

            Assert.Equal("AsyncOk", resultado);
        }

        [Fact]
        public void Deve_Obter_Metodo_De_Interface()
        {
            var metodo = typeof(ClasseComInterface).ObterMetodo("MetodoDaInterface");

            Assert.NotNull(metodo);
            Assert.Equal("MetodoDaInterface", metodo.Name);
            Assert.Equal(typeof(ClasseComInterface), metodo.DeclaringType);
        }

        [Fact]
        public void Deve_Retornar_Null_Se_Metodo_Nao_Existir()
        {
            Type tipo = typeof(ClasseSemMetodo);

            MethodInfo metodo = tipo.ObterMetodo("MetodoAsync");

            Assert.Null(metodo);
        }

        [Fact]
        public void Deve_Obter_Metodo_Da_Classe_Heranca()
        {
            var metodo = typeof(ClasseHeranca).ObterMetodo("MetodoDaInterface");

            Assert.NotNull(metodo);
            Assert.Equal("MetodoDaInterface", metodo.Name);
        }

        [Fact]
        public void Deve_Obter_Metodo_De_Interface_Herdada()
        {
            var metodo = typeof(ClasseInterfaceFilha).ObterMetodo("MetodoBase");

            Assert.NotNull(metodo);
            Assert.Equal("MetodoBase", metodo.Name);
        }

        [Fact]
        public void Deve_Obter_Metodo_De_Classe_Base_Abstrata()
        {
            var metodo = typeof(ClasseDerivada).ObterMetodo("MetodoAbstrato");

            Assert.NotNull(metodo);
            Assert.Equal("MetodoAbstrato", metodo.Name);
        }

        [Fact]
        public void Nao_Deve_Obter_Metodo_Da_Interface_Com_Classe_Abstrata()
        {
            var metodo = typeof(ClasseAbstrataInterface).ObterMetodo("MetodoDaInterface");

            Assert.Null(metodo);
        }

        [Fact]
        public void Deve_Entrar_No_Break_Do_Foreach()
        {
            var metodo = typeof(ClasseComMultiplasInterfaces).ObterMetodo("MetodoInterface2");

            Assert.NotNull(metodo);
            Assert.Equal("MetodoInterface2", metodo.Name);
            Assert.Equal(typeof(ClasseComMultiplasInterfaces), metodo.DeclaringType);
        }
    }
}
