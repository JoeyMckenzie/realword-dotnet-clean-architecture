namespace Conduit.Core.Articles.Queries.GetFeed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using Extensions;
    using Infrastructure;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Internal;
    using Persistence;

    public class GetFeedQueryHandler : IRequestHandler<GetFeedQuery, ArticleViewModelList>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;

        public GetFeedQueryHandler(
            ICurrentUserContext currentUserContext,
            ConduitDbContext context,
            IMapper mapper)
        {
            _currentUserContext = currentUserContext;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ArticleViewModelList> Handle(GetFeedQuery request, CancellationToken cancellationToken)
        {
            // Get the use on the request
            var currentUser = await _currentUserContext.GetCurrentUserContext();
            var emptyFeedResults = new ArticleViewModelList
            {
                Articles = new List<ArticleDto>()
            };

            // Get the list of all users followed by the current user
            var followedUsers = await _context.UserFollows
                .Where(uf => uf.UserFollower == currentUser)
                .Include(uf => uf.UserFollowing)
                    .ThenInclude(a => a.Articles)
                .ToListAsync(cancellationToken);

            if (!followedUsers.Any())
            {
                return emptyFeedResults;
            }

            // Get all articles from the followed users
            var feedArticles = followedUsers
                .SelectMany(uf => uf.UserFollowing.Articles)
                .Skip(request.Offset)
                .Take(request.Limit)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            // Map the articles to the view model list
            var articleViewModelList = new ArticleViewModelList
            {
                Articles = _mapper.Map<IEnumerable<ArticleDto>>(feedArticles)
            };
            articleViewModelList.SetFollowingAndFavorited(feedArticles, currentUser);

            return articleViewModelList;
        }
    }
}