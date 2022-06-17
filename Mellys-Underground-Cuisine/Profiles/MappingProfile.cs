using AutoMapper;
using DAL.Entities;
using Mellys_Underground_Cuisine.Models.ViewModels;

namespace Mellys_Underground_Cuisine.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, AppUserVM>().ReverseMap();
            CreateMap<Dish, DishVM>().ReverseMap();
            CreateMap<Dish, IndexVM>().ReverseMap();
            CreateMap<Ingredient, AddIngredientVM>().ReverseMap();
        }
    }
}
