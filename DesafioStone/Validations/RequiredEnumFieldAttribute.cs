using System.ComponentModel.DataAnnotations;

namespace DesafioStone.Validations
{
    public class RequiredEnumFieldAttribute : RequiredAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null)
            {
                return new ValidationResult($"The field {context.DisplayName} is not a string be null");
            }

            var type = value.GetType();

            if (!type.IsEnum)
                return new ValidationResult($"The field {context.DisplayName} is not an Enum");

            if (!Enum.IsDefined(type, value))
                return new ValidationResult($"The field {context.DisplayName} value is not defined in the Enum");

            return ValidationResult.Success;
        }
    }
}
