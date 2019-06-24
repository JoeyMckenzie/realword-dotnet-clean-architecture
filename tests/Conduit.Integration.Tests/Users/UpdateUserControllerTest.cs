namespace Conduit.Integration.Tests.Users
{
    using System.Threading.Tasks;
    using Core.Users.Commands.UpdateUser;
    using Domain.Dtos;
    using Domain.Dtos.Users;
    using Infrastructure;
    using Xunit;

    public class UpdateUserControllerTest : ControllerBaseTestFixture
    {
        private const string UpdateUserEndpoint = "/api/user";

        [Fact]
        public async Task GivenValidUserUpdateRequest_WhenTheUserIsFound_ReturnsUserViewModelWithSuccessfulResponse()
        {
            // Arrange
            var updateUserRequest = new UpdateUserCommand
            {
                User = new UserDto
                {
                    Email = "my.new.email@gmail.com",
                    
                }
            }
            
            // Act
            
            
            // Assert
        }
    }
}