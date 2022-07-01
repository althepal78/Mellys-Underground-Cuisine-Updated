using DAL.Entities;

namespace Mellys_Underground_Cuisine.Models.ViewModels
{
    public class MenuVM
    {
        public List<Dish> DishList { get; set; } = new List<Dish>();
    }
}
