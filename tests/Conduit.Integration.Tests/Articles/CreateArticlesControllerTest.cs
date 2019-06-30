namespace Conduit.Integration.Tests.Articles
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Articles.Commands.CreateArticle;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class CreateArticlesControllerTest : ControllerBaseTestFixture
    {
        [Fact]
        public async Task GivenValidCreateRequest_WhenTheUserIsVerified_ReturnsArticleViewModelWithSuccessfulResponse()
        {
            // Arrange
            var createArticleCommand = new CreateArticleCommand
            {
                Article = new CreateArticleDto
                {
                    Title = "Why C# is the Best Language",
                    Description = "It really is!",
                    Body = "I love .NET Core",
                    TagList = new[] { "dotnet", "c#" }
                }
            };
            var requestContent = await ContentHelper.GetRequestContentWithAuthorization(createArticleCommand, Client);

            // Act
            var response = await Client.PostAsync(ArticlesEndpoint, requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ArticleViewModel>();
            responseContent.Article.Slug.ShouldBe("why-c-is-the-best-language");
            responseContent.Article.Body.ShouldBe(createArticleCommand.Article.Body);
            responseContent.Article.Title.ShouldBe(createArticleCommand.Article.Title);
            responseContent.Article.Description.ShouldBe(createArticleCommand.Article.Description);
            responseContent.Article.TagList.ShouldContain("dotnet");
            responseContent.Article.TagList.ShouldContain("c#");
        }

        [Fact]
        public async Task GivenValidCreateRequest_WhenTheUserIsNotVerified_ReturnsUnauthorizedResponse()
        {
            // Arrange
            var createArticleCommand = new CreateArticleCommand
            {
                Article = new CreateArticleDto
                {
                    Title = "Why C# is the Best Language",
                    Description = "It really is!",
                    Body = "I love .NET Core",
                    TagList = new[] { "dotnet", "c#" }
                }
            };
            var requestContent = ContentHelper.GetRequestContent(createArticleCommand);

            // Act
            var response = await Client.PostAsync(ArticlesEndpoint, requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
            responseContent.ShouldBeNull();
        }

        [Fact]
        public async Task GivenInvalidCreateRequest_WhenTheUserIsVerified_ReturnsErrorViewModelWithUnsupportedMediaType()
        {
            // Arrange
            var createArticleCommand = new CreateArticleCommand
            {
                Article = new CreateArticleDto
                {
                    Body = "I love .NET Core",
                }
            };
            var requestContent = await ContentHelper.GetRequestContentWithAuthorization(createArticleCommand, Client);

            // Act
            var response = await Client.PostAsync(ArticlesEndpoint, requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
        }

        [Fact]
        public async Task GivenVInvalidCreateRequest_WhenNoTagListItemsAreOnTheRequest_ReturnsUserViewModelWithoutArticleTags()
        {
            // Arrange
            var createArticleCommand = new CreateArticleCommand
            {
                Article = new CreateArticleDto
                {
                    Title = "Why C# is the Best Language",
                    Description = "It really is!",
                    Body = "I love .NET Core"
                }
            };
            var requestContent = await ContentHelper.GetRequestContentWithAuthorization(createArticleCommand, Client);

            // Act
            var response = await Client.PostAsync(ArticlesEndpoint, requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModel>(response);

            // Assert
            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ArticleViewModel>();
            responseContent.Article.Slug.ShouldBe("why-c-is-the-best-language");
            responseContent.Article.Body.ShouldBe(createArticleCommand.Article.Body);
            responseContent.Article.Title.ShouldBe(createArticleCommand.Article.Title);
            responseContent.Article.Description.ShouldBe(createArticleCommand.Article.Description);
            responseContent.Article.TagList.ShouldBeEmpty();
        }
    }
}