using System;
using System.Linq;

namespace Backend.Helpers
{
    public class PasswordValidator
    {
        public static bool IsValidPassword(string password)
        {
            // 1. Check length between 8 and 35
            if (string.IsNullOrWhiteSpace(password)
                || password.Length < 8
                || password.Length > 35)
            {
                return false;
            }

            // 2. Check for uppercase letters
            bool hasUpperCase = password.Any(char.IsUpper);

            // 3. Check for digits
            bool hasDigit = password.Any(char.IsDigit);

            // 4. Check for special characters (anything not letter or digit)
            bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

            // Return true only if all conditions are met
            return hasUpperCase && hasDigit && hasSpecialChar;
        }

    }
}
