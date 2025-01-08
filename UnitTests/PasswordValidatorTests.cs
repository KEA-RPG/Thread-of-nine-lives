using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Backend.Helpers;

namespace UnitTests
{
    public class PasswordValidatorTests
    {
        /// <summary>
        /// Helper method to generate a password of a given length that
        /// *does* satisfy uppercase, digit, and special char (for boundary tests).
        /// This is only used for boundary tests.
        /// </summary>
        private string GeneratePasswordOfLength(int length)
        {
            // If length is 0 or negative, just return empty (invalid by length).
            if (length <= 0)
                return string.Empty;

            // Start with "A1#", covering uppercase, digit, special character.
            // Pad with 'a' to reach the total length if needed.
            var basePart = "A1#";
            if (length <= 3)
                return basePart.Substring(0, length);

            return basePart + new string('a', length - 3);
        }

        #region Boundary Tests (8–35)

        // ----------------------------
        // Positive Boundary Tests
        // ----------------------------

        [Fact]
        public void IsValidPassword_ReturnsTrue_WhenLengthIsExactly8()
        {
            // Arrange
            var password = GeneratePasswordOfLength(8);

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsTrue_WhenLengthIsExactly9()
        {
            // Arrange
            var password = GeneratePasswordOfLength(9);

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsTrue_WhenLengthIsExactly34()
        {
            // Arrange
            var password = GeneratePasswordOfLength(34);

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsTrue_WhenLengthIsExactly35()
        {
            // Arrange
            var password = GeneratePasswordOfLength(35);

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.True(result);
        }

        // ----------------------------
        // Negative Boundary Tests
        // ----------------------------

        [Fact]
        public void IsValidPassword_ReturnsFalse_WhenLengthIs7()
        {
            // Arrange
            var password = GeneratePasswordOfLength(7);

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsFalse_WhenLengthIs36()
        {
            // Arrange
            var password = GeneratePasswordOfLength(36);

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Positive Tests (Valid Passwords)

        // Each of these passwords is intended to be valid:
        //  - 8–35 chars
        //  - At least one uppercase letter
        //  - At least one digit
        //  - At least one special character

        [Fact]
        public void IsValidPassword_ReturnsTrue_For_123456_Hgt6()
        {
            // Arrange
            // 123456"Hgt6 => has digits, uppercase 'H', special char '"', total length = 11
            string password = "123456\"Hgt6";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsTrue_For_012345_Atgg46Quote()
        {
            // Arrange
            // 012345@Tgg46" => digits, uppercase 'T', special chars '@' and '"'
            // length = 13
            string password = "012345@Tgg46\"";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsTrue_For_Minus123456PercentJghh45()
        {
            // Arrange
            // -123456%Jghh45 => length 14, has uppercase 'J', digit '1..6,4,5', 
            // special chars '-' and '%'
            string password = "-123456%Jghh45";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsTrue_For_GthjGT1Quote_gleDoubleStar()
        {
            // Arrange
            // GthjGT1"gle** => 13 chars, uppercase 'G','T', digit '1',
            // special chars '"','*'
            string password = "GthjGT1\"gle**";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsTrue_For_QuoteAtPercentAmpGh12Star3456GH()
        {
            // Arrange
            // "@%&Gh12*3456GH => 15 chars, uppercase 'G','H','G','H',
            // digits '1','2','3','4','5','6', special chars '"','@','%','&','*'
            string password = "\"@%&Gh12*3456GH";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region Negative Tests (Invalid Passwords)

        // Each of these should fail either for missing uppercase,
        // missing digit, missing special char, empty string, or invalid length.

        [Fact]
        public void IsValidPassword_ReturnsFalse_For_1234567891011()
        {
            // Arrange
            // 1234567891011 => has digits, but no uppercase, no special char
            string password = "1234567891011";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsFalse_For_hellohelloHello1_NoSpecialChar()
        {
            // Arrange
            // hellohelloHello1 => has uppercase 'H', digit '1', 
            // but no special character
            string password = "hellohelloHello1";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsFalse_For_NAt34567_TooShort()
        {
            // Arrange
            // N@34567 => 7 chars (too short), although it does have uppercase 'N',
            // digit '3,4,5,6,7', and special char '@'
            string password = "N@34567";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsFalse_For_VeryLongString_Exceeds35Chars()
        {
            // Arrange
            // This string is well over 35 characters, so it should fail by length
            string password = "}5GA3c=o709&;QS6{bjWb#U(z*sW1K18d;v[3mLD";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsFalse_For_hellohelloHelloStar_NoDigit()
        {
            // Arrange
            // hellohelloHello* => has uppercase 'H', special char '*', 
            // length is okay, but there's no digit
            string password = "hellohelloHello*";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void IsValidPassword_ReturnsFalse_For_EmptyPassword()
        {
            // Arrange
            // " " -> whitespace (effectively empty/invalid)
            string password = " ";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }


        #endregion
    }
}
