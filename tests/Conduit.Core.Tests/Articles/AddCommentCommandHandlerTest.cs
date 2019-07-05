namespace Conduit.Core.Tests.Articles
{
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Articles.Commands.AddComment;
    using Domain.Dtos.Comments;
    using Domain.ViewModels;
    using Exceptions;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using Xunit;

    public class AddCommentCommandHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheArticleExists_AddsCommentToArticle()
        {
            // Arrange
            var commentDto = new AddCommentDto
            {
                Body = "This article sucks!"
            };
            var addCommentCommand = new AddCommentCommand("how-to-train-your-dragon", commentDto);
            var articleComments = Context.Articles
                .Include(a => a.Comments)
                .FirstOrDefault(a => a.Slug == "how-to-train-your-dragon")?
                .Comments;
            articleComments?.Count.ShouldBe(2);
            articleComments?.ShouldNotContain(c => c.Body == "This article sucks!");

            // Act
            var handler = new AddCommentCommandHandler(CurrentUserContext, Context, Mapper, MachineDateTime);
            var response = await handler.Handle(addCommentCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<CommentViewModel>();
            response.Comment.ShouldNotBeNull();
            response.Comment.ShouldBeOfType<CommentDto>();
            articleComments?.Count.ShouldBe(3);
            articleComments?.ShouldContain(c => c.Body == "This article sucks!");
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleDoesNotExist_ThrowsApiException()
        {
            // Arrange
            var commentDto = new AddCommentDto
            {
                Body = "This article sucks!"
            };
            var addCommentCommand = new AddCommentCommand("this-article-sucks", commentDto);

            // Act
            var handler = new AddCommentCommandHandler(CurrentUserContext, Context, Mapper, MachineDateTime);
            var response = await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await handler.Handle(addCommentCommand, CancellationToken.None);
            });

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ConduitApiException>();
        }
    }
}