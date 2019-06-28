namespace Conduit.Core.Infrastructure
{
    using System.Linq;
    using AutoMapper;
    using Domain.Dtos;
    using Domain.Dtos.Articles;
    using Domain.Dtos.Users;
    using Domain.Entities;
    using Domain.ViewModels;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<UserRegistrationDto, UserDto>();

            CreateMap<ConduitUser, UserDto>()
                .ForMember(u => u.Username, m => m.MapFrom(cu => cu.UserName))
                .ForMember(u => u.Email, m => m.MapFrom(cu => cu.Email));

            // Author mappings
            CreateMap<ConduitUser, AuthorDto>()
                .ForMember(a => a.Bio, m => m.MapFrom(cu => cu.Bio))
                .ForMember(a => a.Image, m => m.MapFrom(cu => cu.Image))
                .ForMember(a => a.Username, m => m.MapFrom(cu => cu.UserName));

            // Article mappings
            CreateMap<Article, ArticleDto>()
                .ForMember(a => a.TagList, m => m.MapFrom(a => a.ArticleTags.Select(at => at.Tag.Description)));

            // Tag mappings
            CreateMap<Tag, TagDto>();

            // Profile Mappings
            CreateMap<ConduitUser, ProfileDto>()
                .ForMember(u => u.Username, m => m.MapFrom(u => u.UserName))
                .ForMember(u => u.Bio, m => m.MapFrom(u => u.Bio))
                .ForMember(u => u.Image, m => m.MapFrom(u => u.Image));
        }
    }
}