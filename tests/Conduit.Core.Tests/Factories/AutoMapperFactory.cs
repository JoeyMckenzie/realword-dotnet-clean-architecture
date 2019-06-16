namespace Conduit.Core.Tests.Factories
{
    using AutoMapper;
    using Core.Infrastructure;

    public static class AutoMapperFactory
    {
        public static IMapper Create()
        {
            var mappingConfig = new MapperConfiguration(configuration => configuration.AddProfile(new MappingProfile()));
            return mappingConfig.CreateMapper();
        }
    }
}