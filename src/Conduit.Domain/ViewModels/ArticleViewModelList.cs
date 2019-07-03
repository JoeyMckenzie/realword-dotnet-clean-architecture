namespace Conduit.Domain.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Dtos;
    using Dtos.Articles;

    public class ArticleViewModelList
    {
        public IEnumerable<ArticleDto> Articles { get; set; }

        public int ArticlesCount => Articles.Count();
    }
}