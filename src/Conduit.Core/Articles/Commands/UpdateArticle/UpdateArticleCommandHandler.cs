namespace Conduit.Core.Articles.Commands.UpdateArticle
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using Exceptions;
    using Infrastructure;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Persistence;
    using Shared;
    using Shared.Extensions;

    public class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, ArticleViewModel>
    {
        private readonly ILogger<UpdateArticleCommandHandler> _logger;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ConduitDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly IMapper _mapper;

        public UpdateArticleCommandHandler(
            ILogger<UpdateArticleCommandHandler> logger,
            ConduitDbContext context,
            ICurrentUserContext currentUserContext,
            IMapper mapper,
            IDateTime dateTime)
        {
            _logger = logger;
            _context = context;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
            _dateTime = dateTime;
        }

        public async Task<ArticleViewModel> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the current user and the article in question to update
            var currentUser = await _currentUserContext.GetCurrentUserContext();
            var articleToUpdate = await _context.Articles
                .Where(a => string.Equals(a.Slug, request.Slug, StringComparison.OrdinalIgnoreCase))
                .Include(a => a.ArticleTags)
                    .ThenInclude(at => at.Tag)
                .Include(a => a.Author)
                .FirstOrDefaultAsync(a => string.Equals(a.AuthorId, currentUser.Id, StringComparison.OrdinalIgnoreCase), cancellationToken);

            // Invalidate the request if the article was not found
            if (articleToUpdate == null)
            {
                throw new ConduitApiException($"No article found with slug [{request.Slug}]", HttpStatusCode.NotFound);
            }

            // Invalidate the request if the user is not the author of the article
            if (!string.Equals(articleToUpdate.AuthorId, currentUser.Id, StringComparison.OrdinalIgnoreCase))
            {
                throw new ConduitApiException($"User [{currentUser.Email}] does not own article [{request.Slug}] and may not update it", HttpStatusCode.BadRequest);
            }

            // Update the article slug, if it exists
            if (request.Article.Title != null)
            {
                articleToUpdate.Title = request.Article.Title;
                articleToUpdate.Slug = request.Article.Title.ToSlug();
            }

            // Update the article the other properties
            articleToUpdate.Body = request.Article.Body ?? articleToUpdate.Body;
            articleToUpdate.Description = request.Article.Description ?? articleToUpdate.Description;
            articleToUpdate.UpdatedAt = _dateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Article [{request.Slug}] updated by user [{currentUser.Email}]");

            // Map to the view model
            var viewModel = new ArticleViewModel
            {
                Article = _mapper.Map<ArticleDto>(articleToUpdate)
            };

            return viewModel;
        }
    }
}