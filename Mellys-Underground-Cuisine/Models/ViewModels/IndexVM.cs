using DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mellys_Underground_Cuisine.Models.ViewModels
{
    public class IndexVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public ICollection<DishIngredient>? DishIngredient { get; set; }
        public int Quantity { get; set; }
               
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Price { get; set; }
        public string FilePath { get; set; }
    }
}
