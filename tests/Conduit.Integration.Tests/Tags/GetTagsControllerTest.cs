namespace Conduit.Integration.Tests.Tags
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class GetTagsControllerTest : ControllerBaseTestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenNoAuthenticationIsRequired_ReturnsTagViewModelWithSuccessfulResponse()
        {
            // Act
            var response = await Client.GetAsync(TagsEndpoint);
            var responseContent = await ContentHelper.GetResponseContent<TagViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<TagViewModel>();
            responseContent.Tags.ShouldNotBeNull();
            responseContent.Tags.ShouldBeOfType<List<string>>();
            responseContent.Tags.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GivenValidRequest_WhenNoAuthenticationIsRequiredAndNoTagsExist_ReturnsEmptyTagViewModelWithSuccessfulResponse()
        {
            // Act
            var response = await Client.GetAsync(TagsEndpoint);
            var responseContent = await ContentHelper.GetResponseContent<TagViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<TagViewModel>();
            responseContent.Tags.ShouldNotBeNull();
            responseContent.Tags.ShouldBeOfType<List<Tag>>();
            responseContent.Tags.ShouldBeEmpty();
        }
    }
}