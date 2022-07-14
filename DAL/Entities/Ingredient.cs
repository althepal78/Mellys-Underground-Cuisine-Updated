using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Ingredient
    {
        [Key]
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string? NormalizeName { get; set; }

        public ICollection<DishIngredient> DishIngredient { get; set; }
    }
}
