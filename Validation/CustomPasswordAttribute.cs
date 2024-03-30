using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Product_Management.Validation
{
    public class CustomPasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            string password = value.ToString();

            // Password length between 8 to 16 characters
            if (password.Length < 8 || password.Length > 16)
                return false;

            // Must contain at least one uppercase letter
            if (!Regex.IsMatch(password, "[A-Z]"))
                return false;

            // Must contain at least one lowercase letter
            if (!Regex.IsMatch(password, "[a-z]"))
                return false;

            // Must contain at least one digit
            if (!Regex.IsMatch(password, "[0-9]"))
                return false;

            // Must contain at least one special character
            if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
                return false;

            return true;
        }
    }
}
