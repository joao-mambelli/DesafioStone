using System.ComponentModel.DataAnnotations;

namespace DesafioStone.CustomAttributes
{
    public class ValidDocumentAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            if (!value.GetType().Equals(typeof(string)))
                return false;

            var str = (string)value;

            foreach (var c in str)
                if (str.Where(x => x == c).Count() == str.Length)
                    return false;

            if (str.Length == 11)
            {
                var soma = 0;
                for (int i = 0; i < 9; i++)
                    soma += (str[i] - '0') * (10 - i);

                var modulo = soma % 11;
                var resultado = modulo < 2 ? 0 : 11 - modulo;

                if (resultado != str[9] - '0')
                {
                    return false;
                }

                soma = 0;
                for (int i = 0; i < 10; i++)
                    soma += (str[i] - '0') * (11 - i);

                modulo = soma % 11;
                resultado = modulo < 2 ? 0 : 11 - modulo;

                return resultado == str[10] - '0';
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
                    return false;
                }

                soma = 0;
                for (int i = 0; i < 13; i++)
                    soma += (str[i] - '0') * ((6 - i) < 2 ? (14 - i) : (6 - i));

                modulo = soma % 11;
                resultado = modulo < 2 ? 0 : 11 - modulo;

                return resultado == str[13] - '0';
            }
        }
    }
}
