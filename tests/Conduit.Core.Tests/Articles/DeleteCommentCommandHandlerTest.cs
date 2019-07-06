namespace Conduit.Core.Tests.Articles
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Articles.Commands.DeleteComment;
    using Exceptions;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class DeleteCommentCommandHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheCommentExistsAndIsOwnerByTheRequest_DeletesCommentSuccessfully()
        {
            // Arrange
            var deleteCommentCommand = new DeleteCommentCommand(1, "how-to-train-your-dragon");
            var existingComment = Context.Comments.Find(1);
            existingComment.ShouldNotBeNull();

            // Act
            var handler = new DeleteCommentCommandHandler(Context, CurrentUserContext);
            var response = await handler.Handle(deleteCommentCommand, CancellationToken.None);

            // Assert
            Context.Comments.Find(1).ShouldBeNull();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheCommentDoesNotExistAndIsOwnerByTheRequest_ThrowsApiExceptionForNotFound()
        {
            // Arrange
            var deleteCommentCommand = new DeleteCommentCommand(11, "how-to-train-your-dragon");

            // Act
            var handler = new DeleteCommentCommandHandler(Context, CurrentUserContext);
            var response = await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await handler.Handle(deleteCommentCommand, CancellationToken.None);
            });

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheCommentIsNotOwnedByTheRequester_ThrowsApiExceptionForForbidden()
        {
            // Arrange
            var deleteCommentCommand = new DeleteCommentCommand(2, "how-to-train-your-dragon");

            // Act
            var handler = new DeleteCommentCommandHandler(Context, CurrentUserContext);
            var response = await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await handler.Handle(deleteCommentCommand, CancellationToken.None);
            });

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleDoesNotExist_ThrowsApiExceptionForNotFound()
        {
            // Arrange
            var deleteCommentCommand = new DeleteCommentCommand(1, "how-to-not-train-your-dragon");

            // Act
            var handler = new DeleteCommentCommandHandler(Context, CurrentUserContext);
            var response = await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await handler.Handle(deleteCommentCommand, CancellationToken.None);
            });

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}