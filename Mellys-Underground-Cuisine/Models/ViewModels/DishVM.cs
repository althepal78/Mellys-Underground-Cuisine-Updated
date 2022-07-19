using DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mellys_Underground_Cuisine.Models.ViewModels
{
    public class DishVM
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        [MinLength(5)]
        public string Description { get; set; }

        public DishIngredient? dishIngredient { get; set; }

        [Required]
        public string DishType { get; set; }

        public int Servings { get; set; }

        [Required]
        [Column(TypeName = "smallmoney")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public ICollection<DishIngredient>? DishIngredient { get; set; }
        public ICollection<MenuDish>? MenuDish { get; set; }
        public DateTime date { get; set; }

        public string? FilePath { get; set; }

        [NotMapped]
        public IFormFile? FoodImage { get; set; }

        public bool IsDefualting { get; set; } = false;

    }
}
