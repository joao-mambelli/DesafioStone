using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DesafioStone.Validations
{
    public class ValidDocumentAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null)
                return ValidationResult.Success;

            if (!value.GetType().Equals(typeof(string)))
                return new ValidationResult($"The field {context.DisplayName} is not a string");

            var str = (string)value;

            var rg = new Regex(@"^[0-9]{11}$|^[0-9]{14}$");
            if (!rg.IsMatch(str))
                return new ValidationResult($"Document must be a string composed of 11 or 14 digits only");

            foreach (var c in str)
                if (str.Where(x => x == c).Count() == str.Length)
                    return new ValidationResult($"Invalid Document");

            if (str.Length == 11)
            {
                var soma = 0;
                for (int i = 0; i < 9; i++)
                    soma += (str[i] - '0') * (10 - i);

                var modulo = soma % 11;
                var resultado = modulo < 2 ? 0 : 11 - modulo;

                if (resultado != str[9] - '0')
                {
                    return new ValidationResult($"Invalid Document");
                }

                soma = 0;
                for (int i = 0; i < 10; i++)
                    soma += (str[i] - '0') * (11 - i);

                modulo = soma % 11;
                resultado = modulo < 2 ? 0 : 11 - modulo;

                return resultado == str[10] - '0'
                    ? ValidationResult.Success
                    : new ValidationResult($"Invalid Document");
            }
            else
            {
                var soma = 0;
                for (int i = 0; i < 12; i++)
                    soma += (str[i] - '0') * ((5 - i) < 2 ? (13 - i) : (5 - i));

                var modulo = soma % 11;
                var resultado = modulo < 2 ? 0 : 11 - modulo;

                if (resultado != str[12] - '0')
                {
                    return new ValidationResult($"Invalid Document");
                }

                soma = 0;
                for (int i = 0; i < 13; i++)
                    soma += (str[i] - '0') * ((6 - i) < 2 ? (14 - i) : (6 - i));

                modulo = soma % 11;
                resultado = modulo < 2 ? 0 : 11 - modulo;

                return resultado == str[13] - '0'
                    ? ValidationResult.Success
                    : new ValidationResult($"Invalid Document");
            }
        }
    }
}
