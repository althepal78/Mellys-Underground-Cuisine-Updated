﻿using DAL.Entities;
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
        [MinLength(45)]
        public string Information { get; set; }

        public DishIngredient? dishIngredient { get; set; }

        [Required]
        public int? Quantity { get; set; } 

        [Required]
        public decimal Price { get; set; }

        public string? FilePath { get; set; }

        [NotMapped]
        public IFormFile? FoodImage { get; set; }

        public bool IsDefualting { get; set; } = false;
             
    }
}
