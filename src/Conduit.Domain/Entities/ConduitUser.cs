namespace Conduit.Domain.Entities
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class ConduitUser : IdentityUser
    {
        public ConduitUser()
        {
            Followers = new List<UserFollow>();
            Following = new List<UserFollow>();
            Favorites = new List<Favorite>();
            Articles = new List<Article>();
        }

        public string Bio { get; set; }

        public string Image { get; set; }

        public ICollection<UserFollow> Followers { get; }

        public ICollection<UserFollow> Following { get; }

        public ICollection<Favorite> Favorites { get; }

        public ICollection<Article> Articles { get; }
    }
}