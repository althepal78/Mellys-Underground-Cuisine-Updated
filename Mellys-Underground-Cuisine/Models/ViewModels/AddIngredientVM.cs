using DAL.Entities;

namespace Mellys_Underground_Cuisine.Models.ViewModels
{
    public class AddIngredientVM
    {
        public Guid ID { get; set; }
        public Guid DishId { get; set; }
        public string Name { get; set; }
        public string? NormalizeName { get; set; }
        public List<Ingredient>? Ingredient { get; set; }
    }
}