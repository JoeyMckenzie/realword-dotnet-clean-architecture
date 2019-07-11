namespace Conduit.Domain.ViewModels
{
    using System.Collections.Generic;
    using Entities;

    public class TagViewModel
    {
        public TagViewModel()
        {
            Tags = new List<Tag>();
        }

        public ICollection<Tag> Tags { get; }
    }
}