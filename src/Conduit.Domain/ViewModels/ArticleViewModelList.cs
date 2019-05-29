namespace Conduit.Domain.ViewModels
{
    using System.Collections.Generic;
    using Dtos;

    public class ArticleViewModelList
    {
        public ICollection<ArticleDto> Articles { get; set; }
    }
}