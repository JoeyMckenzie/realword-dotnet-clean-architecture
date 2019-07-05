namespace Conduit.Domain.ViewModels
{
    using System.Collections.Generic;
    using Dtos;
    using Dtos.Comments;

    public class CommentViewModelList
    {
        public IEnumerable<CommentDto> Comments { get; set; }
    }
}