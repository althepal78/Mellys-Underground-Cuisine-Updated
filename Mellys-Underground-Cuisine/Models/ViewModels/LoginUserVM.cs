using System.ComponentModel.DataAnnotations;

namespace Mellys_Underground_Cuisine.Models.ViewModels
{
    public class LoginUserVM
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
