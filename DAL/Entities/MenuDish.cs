using Microsoft.EntityFrameworkCore;

namespace DAL.Entities
{
    [Keyless]
    public class MenuDish
    {
        public Dish Dish { get; set; }
        public Menu Menu { get; set; }
        public Guid DishId { get; set; }
        public Guid MenuId { get; set; }

        //So each dish has a number of servings of that dish
        public int Servings { get; set; }
    }
}
