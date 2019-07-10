namespace Conduit.Integration.Tests
{
    using Core.Users.Commands.LoginUser;
    using Domain.Dtos;
    using Domain.Dtos.Users;

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

        public static readonly LoginUserCommand BackupUser = new LoginUserCommand
        {
            User = new UserLoginDto
            {
                Email = "test.user2@gmail.com",
                Password = "#password3!"
            }
        };
    }
}