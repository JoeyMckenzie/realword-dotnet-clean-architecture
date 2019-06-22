namespace Conduit.Api.Controllers
{
    using System.Threading.Tasks;
    using Core.Articles.Commands.CreateArticle;
    using Core.Articles.Commands.UpdateArticle;
    using Domain.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(typeof(ArticleViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
        public async Task<ArticleViewModel> CreateArticle([FromBody] CreateArticleCommand command)
        {
            _logger.LogInformation($"Creating article for request [{command.Article.Title}]");
            return await Mediator.Send(command);
        }

        [HttpPut("{slug}")]
        [ProducesResponseType(typeof(ArticleViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status404NotFound)]
        public async Task<ArticleViewModel> UpdateArticle([FromBody] UpdateArticleCommand command, string slug)
        {
            _logger.LogInformation($"Updating article [{slug}]");
            command.Slug = slug;
            return await Mediator.Send(command);
        }
    }
}