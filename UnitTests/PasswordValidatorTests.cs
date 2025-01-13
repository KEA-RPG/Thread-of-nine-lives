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
        #region Boundary Tests (8–35) 

        // ----------------------------
        // Positive Boundary Tests
        // ----------------------------

        [Fact]
        public void IsValidPassword_ReturnsTrue_WhenLengthIsExactly8()
        {
            // Arrange
            var password = "A1#aaaaa"; //length is 8

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsTrue_WhenLengthIsExactly9()
        {
            // Arrange
            var password = "A1#aaaaaa"; //length is 9

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsTrue_WhenLengthIsExactly34()
        {
            // Arrange
            var password = "A1#aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"; //length is 34

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsTrue_WhenLengthIsExactly35()
        {
            // Arrange
            var password = "A1#aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"; //length is 35

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
            var password = "A1#aaaa"; //length is 7
            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsFalse_WhenLengthIs36()
        {
            // Arrange
            var password = "A1#aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"; //length is 36

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
        public void IsValidPassword_ReturnsFalse_For_gf1234567891011()
        {
            // Arrange
            // gf1234567891011* => has digits, but no uppercase
            string password = "gf1234567891011*";

            // Act
            bool result = PasswordValidator.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_ReturnsFalse_For_Hjelmhjelm()
        {
            // Arrange
            // Hjelmhjelm* => has uppercase, but no numbers
            string password = "Hjelmhjelm*";

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
