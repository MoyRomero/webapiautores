using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Validaciones
{
    public class FirstLetterUCAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var FirstLetter = value.ToString()[0].ToString();

            if(FirstLetter != FirstLetter.ToUpper())
            {
                return new ValidationResult("La primera letra del campo, debe ser mayúscula.");
            }

            return ValidationResult.Success;
        }
    }
}
