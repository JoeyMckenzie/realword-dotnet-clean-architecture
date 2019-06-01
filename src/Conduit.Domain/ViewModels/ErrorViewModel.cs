namespace Conduit.Domain.ViewModels
{
    using Dtos;

    public class ErrorViewModel
    {
        public ErrorViewModel(object errors)
        {
            Errors = errors;
        }

        public object Errors { get; }
    }
}