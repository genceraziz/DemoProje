using AutoMapper;
using InGameDemo.Core.Models;
using InGameDemo.WebApi.Data;

namespace InGameDemo.WebApi.Infrastructure.AutoMapperImplement
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<Categories, CategoryViewForDto>().ReverseMap();
        }
    }
}
