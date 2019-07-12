namespace Conduit.Core.Tests.Tags
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Tags.Queries.GetTags;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class GetTagsQueryHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenMultipleTagsExist_ReturnsPopulatedTagViewModel()
        {
            // Arrange
            var getQueryTags = new GetTagsQuery();

            // Act
            var handler = new GetTagsQueryHandler(Context, Mapper);
            var response = await handler.Handle(getQueryTags, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<TagViewModelList>();
            response.Tags.ShouldNotBeNull();
            response.Tags.ShouldBeOfType<List<string>>();
            response.Tags.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GivenValidRequest_WhenNoTagsExist_ReturnsEmptyTagList()
        {
            // Arrange
            var getQueryTags = new GetTagsQuery();
            var tagsToRemove = Context.Tags.ToList();
            Context.Tags.RemoveRange(tagsToRemove);
            Context.SaveChanges();

            // Act
            var handler = new GetTagsQueryHandler(Context, Mapper);
            var response = await handler.Handle(getQueryTags, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<TagViewModelList>();
            response.Tags.ShouldNotBeNull();
            response.Tags.ShouldBeOfType<List<string>>();
            response.Tags.ShouldBeEmpty();
        }
    }
}