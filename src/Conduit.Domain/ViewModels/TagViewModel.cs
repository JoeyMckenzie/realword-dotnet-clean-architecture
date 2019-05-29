namespace Conduit.Domain.ViewModels
{
    using System.Collections.Generic;
    using Entities;

    public class TagViewModel
    {
        public TagViewModel()
        {
            Tags = new HashSet<Tag>();
        }

        public ISet<Tag> Tags { get; }
    }
}