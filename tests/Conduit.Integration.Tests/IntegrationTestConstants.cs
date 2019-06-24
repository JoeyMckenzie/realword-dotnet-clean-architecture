namespace Conduit.Integration.Tests
{
    using Core.Users.Commands.LoginUser;
    using Domain.Dtos;

    public static class IntegrationTestConstants
    {
        public static readonly LoginUserCommand PrimaryUser = new LoginUserCommand
        {
            User = new UserLoginDto
            {
                Email = "joey.mckenzie@gmail.com",
                Password = "#password1!"
            }
        };

        public static readonly LoginUserCommand SecondaryUser = new LoginUserCommand
        {
            User = new UserLoginDto
            {
                Email = "test.user@gmail.com",
                Password = "#passwordTwo1!"
            }
        };
    }
}