namespace Conduit.Domain.Dtos.Comments
{
    using System;

    public class CommentDto
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Body { get; set; }

        public AuthorDto Author { get; set; }
    }
}