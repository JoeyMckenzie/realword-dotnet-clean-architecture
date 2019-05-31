namespace Conduit.Domain.Entities
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class ConduitUser : IdentityUser
    {
        public ConduitUser()
        {
            Followers = new HashSet<UserFollow>();
            Following = new HashSet<UserFollow>();
            Favorites = new HashSet<Favorite>();
        }

        public string Bio { get; set; }

        public string Image { get; set; }

        public ISet<UserFollow> Followers { get; }

        public ISet<UserFollow> Following { get; }

        public ISet<Favorite> Favorites { get; }
    }
}