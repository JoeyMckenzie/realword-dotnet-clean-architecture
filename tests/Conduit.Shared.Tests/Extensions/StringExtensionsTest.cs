namespace Conduit.Shared.Tests.Extensions
{
    using Shared.Extensions;
    using Shouldly;
    using Xunit;

    public class StringExtensionsTest
    {
        [Fact]
        public void ExistsAndIsValid_GivenAnEmptyString_ReturnsTrue()
        {
            // Arrange
            var emptyStringValue = string.Empty;

            // Act
            var result = emptyStringValue.ExistsAndIsValid();

            // Assert
            result.ShouldBeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Test")]
        [InlineData("NotEmpty")]
        public void ExistsAndIsValid_GivenNullOrNonEmptyValues_ReturnsFalse(string value)
        {
            // Arrange/Act
            var result = value.ExistsAndIsValid();

            // Assert
            result.ShouldBeFalse();
        }
    }
}