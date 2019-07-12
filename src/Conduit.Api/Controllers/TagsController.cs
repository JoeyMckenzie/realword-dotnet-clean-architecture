namespace Conduit.Api.Controllers
{
    using System.Threading.Tasks;
    using Core.Tags.Queries.GetTags;
    using Domain.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class TagsController : ConduitControllerBase
    {
        private readonly ILogger<TagsController> _logger;

        public TagsController(ILogger<TagsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(TagViewModelList), StatusCodes.Status200OK)]
        public async Task<TagViewModelList> GetTagList()
        {
            _logger.LogInformation("Retrieving list of tags");
            return await Mediator.Send(new GetTagsQuery());
        }
    }
}