namespace Conduit.Domain.Dtos.Users
{
    public class UserUpdateDto
    {
        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Image { get; set; }

        public string Bio { get; set; }
    }
}