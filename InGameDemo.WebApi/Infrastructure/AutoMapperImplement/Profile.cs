using InGameDemo.Core.Models;
using InGameDemo.WebApi.Data;
using Microsoft.AspNetCore.Identity;

namespace InGameDemo.WebApi.Infrastructure.AutoMapperImplement
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<Categories, CategoryViewForDto>().ReverseMap();
            CreateMap<Products, ProductViewForDto>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<ProductAddForDto, Products>().ForMember(dest => dest.CreateUser, opts => opts.MapFrom(src => src.UserName)).ReverseMap();
            CreateMap<ApplicationUser, UserForDto>();
            CreateMap<IdentityRole, RoleForDto>();
        }
    }
}
