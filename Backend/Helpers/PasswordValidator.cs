using System;
using System.Linq;

namespace Backend.Helpers
{
    public class PasswordValidator
    {
        public static bool IsValidPassword(string password)
        {
            // Check for null or whitespace
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Check length boundaries
            if (password.Length < 8 || password.Length > 35)
                return false;

            // Check for uppercase letters
            if (!password.Any(char.IsUpper))
                return false;

            // Check for digits
            if (!password.Any(char.IsDigit))
                return false;

            // Check for special characters
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                return false;

            // If we got this far, everything is valid
            return true;
        }


    }
}
