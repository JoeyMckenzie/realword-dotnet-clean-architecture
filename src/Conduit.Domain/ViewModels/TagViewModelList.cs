namespace Conduit.Domain.ViewModels
{
    using System.Collections.Generic;

    public class TagViewModelList
    {
        public TagViewModelList()
        {
            Tags = new List<string>();
        }

        public ICollection<string> Tags { get; }
    }
}