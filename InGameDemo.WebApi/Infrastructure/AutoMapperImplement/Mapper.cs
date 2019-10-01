using System.Collections.Generic;

namespace InGameDemo.WebApi.Infrastructure.AutoMapperImplement
{
    public class Mapper : IMapper
    {
        public Mapper()
        {
            var profiles = new List<AutoMapper.Profile> { new Profile() };

            AutoMapper.Mapper.Initialize(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return AutoMapper.Mapper.Map<TDestination>(source);
        }
    }
}
