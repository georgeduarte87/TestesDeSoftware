using Features.Clientes;
using System;
using Xunit;

namespace Features.Tests._02_Fixtures
{
    [Collection(nameof(ClienteCollection))]
    public class ClienteValidoFixtureTests
    {

        /*
         * Exemplo de como injetar via construtor o cliente
         * 

        public Cliente ClienteValido;

        public ClienteFixtureTests()
        {
            ClienteValido = new Cliente(
                Guid.NewGuid(),
                "Eduardo",
                "Pires",
                DateTime.Now.AddYears(-30),
                "edu@edu.com",
                true,
                DateTime.Now);
        }

        [Fact(DisplayName = "Novo Cliente Valido")]
        [Trait("Categoria", "Cliente Trait Testes")]
        public void Cliente_NovoCliente_DeveEstarValidoCtor()
        {
            // Arrange => Subistituido pela informação de cliente no contrutor

            // Act
            var result = ClienteValido.EhValido();

            // Assert
            Assert.True(result);
            Assert.Equal(0, ClienteValido.ValidationResult.Errors.Count);

        } */

        readonly ClienteTestsFixture _clienteTestsFixture;

        public ClienteValidoFixtureTests(ClienteTestsFixture clienteTestsFixture)
        {
            _clienteTestsFixture = clienteTestsFixture;
        }

        [Fact(DisplayName = "Novo Cliente Valido")]
        [Trait("Categoria", "Cliente Trait Testes")]
        public void Cliente_NovoCliente_DeveEstarValido()
        {
            /*
            // Arrange
            var cliente = new Cliente(
                Guid.NewGuid(),
                "Eduardo",
                "Pires",
                DateTime.Now.AddYears(-30),
                "edu@edu.com",
                true,
                DateTime.Now); */

            var cliente = _clienteTestsFixture.GerarClienteValido();

            // Act
            var result = cliente.EhValido();

            // Assert
            Assert.True(result);
            Assert.Equal(0, cliente.ValidationResult.Errors.Count);

        }
    }
}
