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
            var result = emptyStringValue.IsValidEmptyString();

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
            var result = value.IsValidEmptyString();

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void ToSlug_GivenStringPhrase_ReturnsSlugifiedVersion()
        {
            // Arrange
            const string testPhrase = "I love burritos and beer!";
            const string expectedSlug = "i-love-burritos-and-beer";

            // Act
            var actualSlug = testPhrase.ToSlug();

            // Assert
            actualSlug.ShouldBe(expectedSlug);
        }
    }
}