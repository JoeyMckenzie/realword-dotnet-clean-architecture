namespace Conduit.Core.Infrastructure
{
    using AutoMapper;
    using Domain.Dtos;
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
            CreateMap<Article, ArticleDto>();

            // Tag mappings
            CreateMap<Tag, TagDto>();
        }
    }
}