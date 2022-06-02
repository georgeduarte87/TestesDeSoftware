using Xunit;

namespace Demo.Tests
{
    public class AssertStringsTests
    {
        [Fact]
        public void StringTools_UnirNomes_RetornaNomeCompleto()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var nomeCompleto = sut.Unir("Eduardo", "Pires");

            // Assert
            Assert.Equal("Eduardo Pires", nomeCompleto);
        }

        [Fact]
        public void StringTools_UnirNomes_DeveIgnorarCase()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var nomeCompleto = sut.Unir("Eduardo", "Pires");

            // Assert
            Assert.Equal("EDUARDO PIRES", nomeCompleto, true); // um terceiro argumento informa ignorar CASE
        }

        [Fact]
        public void StringTools_UnirNomes_DeveConterTrecho()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var nomeCompleto = sut.Unir("Eduardo", "Pires");

            // Assert
            Assert.Contains("ardo", nomeCompleto);
        }

        [Fact]
        public void StringTools_UnirNomes_DeveComeçarCom()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var nomeCompleto = sut.Unir("Eduardo", "Pires");

            // Assert
            Assert.StartsWith("Edu", nomeCompleto);
        }

        [Fact]
        public void StringTools_UnirNomes_DeveTerminarCom()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var nomeCompleto = sut.Unir("Eduardo", "Pires");

            // Assert
            Assert.EndsWith("res", nomeCompleto);
        }

        [Fact]
        public void StringTools_UnirNomes_ValidarExpressaoRegular()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var nomeCompleto = sut.Unir("Eduardo", "Pires");

            // Assert
            Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", nomeCompleto);
        }
    }
}
