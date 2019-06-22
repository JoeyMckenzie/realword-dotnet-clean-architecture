namespace Conduit.Core.Tests.Articles
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Articles.Commands.UpdateArticle;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using Exceptions;
    using Infrastructure;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Shared.Extensions;
    using Shouldly;
    using Xunit;

    public class UpdateArticleCommandHandlerTest : TestFixture
    {
        private readonly ILogger<UpdateArticleCommandHandler> _logger;

        public UpdateArticleCommandHandlerTest()
        {
            _logger = NullLogger<UpdateArticleCommandHandler>.Instance;
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleSlugIsNotFound_ThrowsConduitApiExceptionForNotFound()
        {
            // Arrange
            var stubArticleDto = new UpdateArticleDto
            {
                Body = "I don't exist",
                Description = "Or do I?",
                Title = "What is Life? 42."
            };

            var updateArticleCommand = new UpdateArticleCommand
            {
                Slug = "this-article-does-not-exist",
                Article = stubArticleDto
            };

            // Act
            var request = new UpdateArticleCommandHandler(_logger, Context, CurrentUserContext, Mapper, MachineDateTime);

            // Assert
            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await request.Handle(updateArticleCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheUserDidNotCreateTheArticle_ThrowsConduitApiExceptionForBadRequest()
        {
            // Arrange
            var stubArticleDto = new UpdateArticleDto
            {
                Body = "This isn't my article!",
                Description = "Or is it?",
                Title = "I'm a hacker trying to hack hacky articles about hacking."
            };

            var updateArticleCommand = new UpdateArticleCommand
            {
                Slug = "how-to-train-your-dragon",
                Article = stubArticleDto
            };

            // Act
            var request = new UpdateArticleCommandHandler(_logger, Context, CurrentUserContext, Mapper, MachineDateTime);

            // Assert
            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await request.Handle(updateArticleCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheUserOwnsTheArticle_ReturnsUpdatedArticleResponse()
        {
            // Arrange, retrieve the original article information
            var stubArticleDto = new UpdateArticleDto
            {
                Body = "This isn't my article!",
                Description = "Or is it?",
                Title = "I'm a hacker trying to hack hacky articles about hacking."
            };

            var updateArticleCommand = new UpdateArticleCommand
            {
                Slug = "why-beer-is-gods-gift-to-the-world",
                Article = stubArticleDto
            };

            // Act
            var request = new UpdateArticleCommandHandler(_logger, Context, CurrentUserContext, Mapper, MachineDateTime);
            var result = await request.Handle(updateArticleCommand, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<ArticleViewModel>();
            result.Article.Body.ShouldBe(stubArticleDto.Body);
            result.Article.Description.ShouldBe(stubArticleDto.Description);
            result.Article.Title.ShouldBe(stubArticleDto.Title);
            result.Article.TagList.ShouldContain("beer");

            // Validate the article in the DB context
            var updatedArticle = Context.Articles.FirstOrDefault(a =>
                string.Equals(a.Slug, stubArticleDto.Title.ToSlug(), StringComparison.OrdinalIgnoreCase));
            updatedArticle.ShouldNotBeNull();
            updatedArticle.Body.ShouldBe(stubArticleDto.Body);
            updatedArticle.Description.ShouldBe(stubArticleDto.Description);
            updatedArticle.Title.ShouldBe(stubArticleDto.Title);
        }

        [Fact]
        public async Task GivenValidRequest_WhenASubsetOfPropertiesIsSent_ReturnsUpdatedArticleResponseWithOriginalPropertiesUnchanged()
        {
            // Arrange, retrieve the original article information
            var stubArticleDto = new UpdateArticleDto
            {
                Title = "I'm a hacker trying to hack hacky articles about hacking."
            };

            var updateArticleCommand = new UpdateArticleCommand
            {
                Slug = "why-beer-is-gods-gift-to-the-world",
                Article = stubArticleDto
            };

            // Act
            var request = new UpdateArticleCommandHandler(_logger, Context, CurrentUserContext, Mapper, MachineDateTime);
            var result = await request.Handle(updateArticleCommand, CancellationToken.None);

            // Assert
            var updatedArticle = Context.Articles.FirstOrDefault(a =>
                string.Equals(a.Slug, stubArticleDto.Title.ToSlug(), StringComparison.OrdinalIgnoreCase));
            updatedArticle.ShouldNotBeNull();
            result.ShouldNotBeNull();
            result.ShouldBeOfType<ArticleViewModel>();
            result.Article.Body.ShouldBe(updatedArticle.Body);
            result.Article.Description.ShouldBe(updatedArticle.Description);
            result.Article.Title.ShouldBe(stubArticleDto.Title);
            result.Article.TagList.ShouldContain("beer");

            // Validate the article in the DB context
            updatedArticle.Body.ShouldBe(updatedArticle.Body);
            updatedArticle.Description.ShouldBe(updatedArticle.Description);
            updatedArticle.Title.ShouldBe(stubArticleDto.Title);
        }
    }
}