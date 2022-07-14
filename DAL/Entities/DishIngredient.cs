using Microsoft.EntityFrameworkCore;

namespace DAL.Entities
{
    [Keyless]
    public class DishIngredient
    {
        public Dish Dish { get; set; }
        public Ingredient Ingredients { get; set; }
        public Guid DishId { get; set; }
        public Guid IngredientsId { get; set; }
    }
}
