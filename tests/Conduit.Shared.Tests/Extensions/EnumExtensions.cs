namespace Conduit.Shared.Tests.Extensions
{
    using System;
    using System.ComponentModel;
    using Shared.Extensions;
    using Shouldly;
    using Xunit;

    public class EnumExtensionsTest
    {
        [Theory]
        [InlineData(GreatestMoviesOfAllTime.WaynesWorld)]
        [InlineData(GreatestMoviesOfAllTime.AmericanPie2)]
        [InlineData(GreatestMoviesOfAllTime.EncinoMan)]
        [InlineData(GreatestMoviesOfAllTime.OceansEleven)]
        public void GetDescription_GivenGreatestMoviesOfAllTimeEnum_ReturnsValidDescription(GreatestMoviesOfAllTime movie)
        {
            // Arrange
            var descriptions = new[]
            {
                "A tale of two young lads changing the world from their basement",
                "A coming of age ale not suitable for everyone",
                "Brendan Frasier at his hunkiest",
                "Danny Ocean is in for one last job with his buddy Bradley Pitts"
            };

            // Act
            var description = movie.GetDescription();

            // Assert
            description.ShouldNotBeNull();
            description.Length.ShouldBePositive();
            descriptions.ShouldContain(description);
        }

        [Theory]
        [InlineData("Wayne's World 2 is the best sequel in existence")]
        [InlineData(1337)]
        [InlineData(true)]
        public void GetDescription_GivenInvalidEnum_ReturnsNull(IConvertible value)
        {
            // Arrange/Act
            var result = value.GetDescription();

            // Assert
            result.ShouldBeNull();
        }
    }

    public enum GreatestMoviesOfAllTime
    {
        [Description("A tale of two young lads changing the world from their basement")]
        WaynesWorld,
        [Description("A coming of age ale not suitable for everyone")]
        AmericanPie2,
        [Description("Brendan Frasier at his hunkiest")]
        EncinoMan,
        [Description("Danny Ocean is in for one last job with his buddy Bradley Pitts")]
        OceansEleven
    }
}