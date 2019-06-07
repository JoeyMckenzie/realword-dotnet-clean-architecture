namespace Conduit.Domain.ViewModels
{
    using Dtos;

    public class ErrorViewModel
    {
        public ErrorViewModel(ErrorDto errors)
        {
            Errors = errors;
        }

        public ErrorDto Errors { get; }
    }
}