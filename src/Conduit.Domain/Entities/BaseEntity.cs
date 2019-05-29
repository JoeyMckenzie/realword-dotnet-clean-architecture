namespace Conduit.Domain.Entities
{
    using System;

    public class BaseEntity
    {
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}