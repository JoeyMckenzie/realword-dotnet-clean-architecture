namespace Conduit.Integration.Tests.Infrastructure
{
    using System.Net.Http;
    using Api;
    using Xunit;

    public class ControllerBaseTestFixture : IClassFixture<ConduitWebApplicationFactory<Startup>>
    {
        private HttpClient _httpClient;

        protected HttpClient Client => _httpClient ?? (_httpClient = new ConduitWebApplicationFactory<Startup>().CreateClient());
    }
}