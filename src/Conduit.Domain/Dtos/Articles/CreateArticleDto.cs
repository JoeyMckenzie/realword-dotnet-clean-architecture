namespace Conduit.Domain.Dtos.Articles
{
    using System.Collections.Generic;

    public class CreateArticleDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public IEnumerable<string> TagList { get; set; }
    }
}