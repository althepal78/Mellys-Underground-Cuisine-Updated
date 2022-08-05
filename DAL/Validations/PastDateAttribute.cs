
using System.ComponentModel.DataAnnotations;

namespace DAL.Validations
{
    public class PastDateAttribute : ValidationAttribute
    {
        public string getErrorMessage() => $"Please Pick a date after today, we can't do weddings in the past";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (DateTime.Now > DateTime.Parse(value.ToString()))
            {
                return new ValidationResult(getErrorMessage());
            }
            return ValidationResult.Success;
        }

    }
}
