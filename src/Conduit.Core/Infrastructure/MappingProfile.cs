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
            CreateMap<UserRegistrationDto, UserDto>();

            CreateMap<ConduitUser, UserDto>()
                .ForMember(vm => vm.Username, m => m.MapFrom(cu => cu.UserName))
                .ForMember(vm => vm.Email, m => m.MapFrom(cu => cu.Email));
        }
    }
}