namespace Conduit.Core.Tests.Infrastructure
{
    using Xunit;

    [CollectionDefinition("ConduitCollectionFixture")]
    public class ConduitCollectionFixture : ICollectionFixture<TestFixture>
    {
    }
}