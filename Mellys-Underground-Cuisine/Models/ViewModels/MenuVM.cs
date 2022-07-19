using DAL.Entities;

namespace Mellys_Underground_Cuisine.Models.ViewModels
{
    public class MenuVM
    {
        public Guid ID { get; set; }
        public Guid DishId { get; set; }
        public DateTime DateColumn { get; set; }
        public int Servings { get; set; }
        public bool IsChecked { get; set; } = false;       
        public List<Dish>? dishes { get; set; }
        public ICollection<MenuDish>? MenuDish { get; set; }
        public List<Menu>? Menu { get; set; }
    }
}