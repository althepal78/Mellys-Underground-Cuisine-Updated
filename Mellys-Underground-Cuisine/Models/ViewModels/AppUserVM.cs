using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mellys_Underground_Cuisine.Models.ViewModels
{
    public class AppUserVM
    {
        [Required, Display(Name = "User Name: ")]
        public string UserName { get; set; }

        [Required, Display(Name = "Email: ")]
        [DataType(DataType.EmailAddress),EmailAddress]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Please enter a valid e-mail adress")]
        public string Email { get; set; }

        [Required, Display(Name = "Password: ")]
        [DataType(DataType.Password)]
       
        public string Password { get; set; }

        [NotMapped]
        [Required, Display(Name = "Confirm Password: ")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirm password don't match")]
        public string ConfirmPassword { get; set; }

    }
}
