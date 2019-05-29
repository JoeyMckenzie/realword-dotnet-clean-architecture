namespace Conduit.Domain.Entities
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public User()
        {
            Followers = new HashSet<UserFollow>();
            Following = new HashSet<UserFollow>();
        }

        public string Username { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }

        public ISet<UserFollow> Followers { get; }

        public ISet<UserFollow> Following { get; }
    }
}