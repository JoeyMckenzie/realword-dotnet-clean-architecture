namespace Conduit.Api.Controllers
{
    using System.Threading.Tasks;
    using Core.Articles.Commands.CreateArticle;
    using Domain.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Authorize]
    public class ArticlesController : ConduitControllerBase
    {
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(ILogger<ArticlesController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ArticleViewModel> CreateArticle([FromBody] CreateArticleCommand command)
        {
            _logger.LogInformation($"Creating article for request [{command.Article.Title}]");
            return await Mediator.Send(command);
        }
    }
}