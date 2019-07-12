namespace Conduit.Integration.Tests.Infrastructure
{
    using System.Net.Http;
    using Api;
    using Xunit;

    public class ControllerBaseTestFixture : IClassFixture<ConduitWebApplicationFactory<Startup>>
    {
        protected const string ArticlesEndpoint = "/api/articles";
        protected const string LoginEndpoint = "/api/users/login";
        protected const string RegisterEndpoint = "/api/users";
        protected const string UpdateUserEndpoint = "/api/user";
        protected const string ProfilesEndpoint = "/api/profiles";
        protected const string TagsEndpoint = "/api/tags";
        private HttpClient _httpClient;

        protected HttpClient Client => _httpClient ?? (_httpClient = new ConduitWebApplicationFactory<Startup>().CreateClient());
    }
}