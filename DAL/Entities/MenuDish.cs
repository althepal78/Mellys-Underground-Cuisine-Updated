

namespace DAL.Entities
{
    public class MenuDish
    {
        public Dish Dish { get; set; }
        public Menu Menu { get; set; }
        public Guid DishId { get; set; }
        public Guid MenuId { get; set; }
    }
}
