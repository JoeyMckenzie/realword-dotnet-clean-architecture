namespace Conduit.Domain.Entities
{
    public class Author : BaseEntity
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }

        public bool Following { get; set; }
    }
}