namespace Conduit.Domain.ViewModels
{
    using System.Collections.Generic;
    using Dtos;

    public class TagViewModel
    {
        public TagViewModel()
        {
            Tags = new List<string>();
        }

        public ICollection<string> Tags { get; }
    }
}