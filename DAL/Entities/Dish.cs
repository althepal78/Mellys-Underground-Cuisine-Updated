using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{

    public class Dish
    {

        // obviously the key of the dish
        [Key]
        public Guid Id { get; set; }

        // The name of the dish
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        //Information about the dis
        [Required]
        [StringLength(500)]
        [MinLength(45)]
        public string Information { get; set; }

        //This collection of dish ingredients is so we can reference the ingredient knowing that it haves a 
        //connection to the ingredient
        public ICollection<DishIngredient> DishIngredient { get; set; }

        //This is the connection to the menu itself similar on how we using the dish ingredient,c
        public ICollection<MenuDish> MenuDish { get; set; }
        
        //price for the dish itself
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Price { get; set; }

        //This for categorizing each dish. So dessert is something sweet, food an actual meal, appetizer = snack, and 
        //Drink would be for special occasions like when they make coquito for christmas etc..
        [Required]
        public string DishType { get; set; }
             
        //this is to determine whether or not they have their own picture or the defaulting one
        public bool IsDefaulting { get; set; } = false;

        //we save the file in a folder and this is the path to that file in the folder
        public string? FilePath { get; set; }
                        
    }
}
