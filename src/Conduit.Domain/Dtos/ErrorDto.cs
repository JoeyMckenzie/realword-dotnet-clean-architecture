namespace Conduit.Domain.Dtos
{
    using System.Collections.Generic;

    public class ErrorDto
    {
        public ErrorDto()
        {
            Body = new HashSet<string>();
        }

        public ICollection<string> Body { get; }
    }
}