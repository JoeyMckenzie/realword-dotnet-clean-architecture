namespace Conduit.Domain.ViewModels
{
    using System.Collections.Generic;
    using Dtos;

    public class CommentViewModelList
    {
        public ICollection<CommentDto> Comments { get; set; }
    }
}