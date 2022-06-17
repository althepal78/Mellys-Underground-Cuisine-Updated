using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{

    public class Dish
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }


        [Required]
        [StringLength(500)]
        [MinLength(45)]
        public string Information { get; set; }
        
        public ICollection<DishIngredient> DishIngredient { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Price { get; set; }
        

        public string FilePath { get; set; }
    }
}
