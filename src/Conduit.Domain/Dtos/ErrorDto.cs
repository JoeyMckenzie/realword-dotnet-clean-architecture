namespace Conduit.Domain.Dtos
{
    public class ErrorDto
    {
        public ErrorDto(string description, object details = null)
        {
            Description = description;
            Details = details;
        }

        public string Description { get; }

        public object Details { get; set; }
    }
}