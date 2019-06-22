namespace Conduit.Domain.ViewModels
{
    using System.Collections.Generic;
    using Dtos;
    using Dtos.Articles;

    public class ArticleViewModelList
    {
        public ICollection<ArticleDto> Articles { get; set; }
    }
}