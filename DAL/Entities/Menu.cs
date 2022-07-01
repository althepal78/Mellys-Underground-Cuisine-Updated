using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Menu
    {
        [Key]
        public Guid ID { get; set; }

        public List<Dish>? MenuDish { get; set; }
    }
}
