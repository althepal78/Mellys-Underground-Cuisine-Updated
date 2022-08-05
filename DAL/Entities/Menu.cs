using DAL.Validations;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Menu
    {
        [Key]
        public Guid ID { get; set; }

        public bool IsChecked { get; set; } = false;

        [PastDate]
        public DateTime DateColumn { get; set; }

        public ICollection<MenuDish>? MenuDish { get; set; }
     }
}
