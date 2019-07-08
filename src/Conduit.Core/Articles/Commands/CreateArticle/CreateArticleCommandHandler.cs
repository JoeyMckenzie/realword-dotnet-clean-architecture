namespace Conduit.Core.Articles.Commands.CreateArticle
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Dtos.Articles;
    using Domain.Entities;
    using Domain.ViewModels;
    using Extensions;
    using Infrastructure;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Shared;
    using Shared.Extensions;

    public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, ArticleViewModel>
    {
        private readonly ILogger<CreateArticleCommandHandler> _logger;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IConduitDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTime _dateTime;

        public CreateArticleCommandHandler(
            ICurrentUserContext currentUserContext,
            IConduitDbContext context,
            ILogger<CreateArticleCommandHandler> logger,
            IMapper mapper,
            IDateTime dateTime)
        {
            _currentUserContext = currentUserContext;
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _dateTime = dateTime;
        }

        public async Task<ArticleViewModel> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            // Grab a reference to the current user making the request and instantiate the new article
            var currentUser = await _currentUserContext.GetCurrentUserContext();
            var newArticle = new Article
            {
                Author = currentUser,
                Title = request.Article.Title,
                Body = request.Article.Body,
                Description = request.Article.Description,
                CreatedAt = _dateTime.Now,
                UpdatedAt = _dateTime.Now,
                Slug = request.Article.Title.ToSlug()
            };

            // Add the article tags to the article if they are attached on the request
            if (request.Article.TagList != null && request.Article.TagList.Any())
            {
                foreach (var tagDescription in request.Article.TagList)
                {
                    // Create the tag if it does not already exist
                    var tag = await _context.Tags.FirstOrDefaultAsync(t => string.Equals(t.Description, tagDescription, StringComparison.OrdinalIgnoreCase), cancellationToken);
                    if (tag == null)
                    {
                        _logger.LogInformation($"Creating new tag [{tagDescription}] for article ID [{newArticle.Id}]");
                        tag = new Tag
                        {
                            Description = tagDescription,
                            CreatedAt = _dateTime.Now,
                            UpdatedAt = _dateTime.Now
                        };
                        await _context.Tags.AddAsync(tag, cancellationToken);
                    }

                    newArticle.ArticleTags.Add(new ArticleTag
                    {
                        Article = newArticle,
                        Tag = tag,
                        CreatedAt = _dateTime.Now,
                        UpdatedAt = _dateTime.Now,
                    });
                }
            }

            // Add the article and tags
            if (newArticle.ArticleTags.Any())
            {
                await _context.ArticleTags.AddRangeAsync(newArticle.ArticleTags, cancellationToken);
            }

            await _context.Articles.AddAsync(newArticle, cancellationToken);
            await _context.AddActivityAsync(
                ActivityType.ArticleCreate,
                TransactionType.Article,
                newArticle.Id.ToString());
            await _context.SaveChangesAsync(cancellationToken);

            // Map to the view model
            var viewModel = new ArticleViewModel
            {
                Article = _mapper.Map<ArticleDto>(newArticle)
            };

            return viewModel;
        }
    }
}