namespace Conduit.Api.Controllers
{
    using System.Threading.Tasks;
    using Core.Articles.Commands.AddComment;
    using Core.Articles.Commands.CreateArticle;
    using Core.Articles.Commands.DeleteArticle;
    using Core.Articles.Commands.DeleteComment;
    using Core.Articles.Commands.FavoriteArticle;
    using Core.Articles.Commands.UnfavoriteArticle;
    using Core.Articles.Commands.UpdateArticle;
    using Core.Articles.Queries.GetArticle;
    using Core.Articles.Queries.GetArticles;
    using Core.Articles.Queries.GetCommentsFromArticle;
    using Core.Articles.Queries.GetFeed;
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

        [HttpDelete("{slug}")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteArticle(string slug)
        {
            _logger.LogInformation($"Deleting article [{slug}]");
            return Ok(await Mediator.Send(new DeleteArticleCommand(slug)));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ArticleViewModelList), StatusCodes.Status200OK)]
        public async Task<ArticleViewModelList> GetArticles(
            [FromQuery] string tag,
            [FromQuery] string author,
            [FromQuery] string favorited,
            [FromQuery] int? limit,
            [FromQuery] int? offset)
        {
            _logger.LogInformation($"Retrieving all articles for query [tag: {tag}] [author: {author}] [favorited: {favorited}]");
            return await Mediator.Send(new GetArticlesQuery(tag, author, favorited, limit, offset));
        }

        [HttpGet("feed")]
        [ProducesResponseType(typeof(ArticleViewModelList), StatusCodes.Status200OK)]
        public async Task<ArticleViewModelList> GetFeed([FromQuery] int? limit, [FromQuery] int? offset)
        {
            _logger.LogInformation("Retrieving feed articles");
            return await Mediator.Send(new GetFeedQuery(limit, offset));
        }

        [HttpGet("{slug}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ArticleViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ArticleViewModel), StatusCodes.Status404NotFound)]
        public async Task<ArticleViewModel> GetArticleFromSlug(string slug)
        {
            _logger.LogInformation($"Retrieving article [{slug}]");
            return await Mediator.Send(new GetArticleQuery(slug));
        }

        [HttpPost("{slug}/comments")]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status415UnsupportedMediaType)]
        public async Task<CommentViewModel> AddCommentToArticle([FromBody] AddCommentCommand command, string slug)
        {
            _logger.LogInformation($"Adding comment to article [{slug}]");
            command.Slug = slug;
            return await Mediator.Send(command);
        }

        [HttpGet("{slug}/comments")]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status404NotFound)]
        public async Task<CommentViewModelList> GetCommentsFromArticle(string slug)
        {
            _logger.LogInformation($"Retrieve all comments from article [{slug}]");
            return await Mediator.Send(new ArticleCommentsQuery(slug));
        }

        [HttpDelete("{slug}/comments/{id}")]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult> GetCommentsFromArticle(int id, string slug)
        {
            _logger.LogInformation($"Removing comment [{id}] from article [{slug}]");
            return Ok(await Mediator.Send(new DeleteCommentCommand(id, slug)));
        }

        [HttpPost("{slug}/favorite")]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status404NotFound)]
        public async Task<ArticleViewModel> FavoriteArticle(string slug)
        {
            _logger.LogInformation($"Favoriting article [{slug}]");
            return await Mediator.Send(new FavoriteArticleCommand(slug));
        }

        [HttpDelete("{slug}/favorite")]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status404NotFound)]
        public async Task<ArticleViewModel> UnfavoriteArticle(string slug)
        {
            _logger.LogInformation($"Removing favorite from article [{slug}]");
            return await Mediator.Send(new UnfavoriteArticleCommand(slug));
        }
    }
}