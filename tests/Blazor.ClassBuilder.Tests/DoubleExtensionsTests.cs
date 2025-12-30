using System.Globalization;
using System.Threading;
using Blazor.ClassBuilder.Extensions;
using Xunit;

namespace Blazor.ClassBuilder.Tests
{
    public class DoubleExtensionsTests
    {
        [Fact]
        public void ToCssString_Integer_ReturnsString()
        {
            // Arrange
            var value = 10.0;

            // Act
            var result = value.ToCssString();

            // Assert
            Assert.Equal("10", result);
        }

        [Fact]
        public void ToCssString_Decimal_ReturnsStringWithDot()
        {
            // Arrange
            var value = 50.5;

            // Act
            var result = value.ToCssString();

            // Assert
            Assert.Equal("50.5", result);
        }

        [Fact]
        public void ToCssString_Zero_ReturnsZero()
        {
            // Arrange
            var value = 0.0;

            // Act
            var result = value.ToCssString();

            // Assert
            Assert.Equal("0", result);
        }

        [Fact]
        public void ToCssString_Negative_ReturnsNegativeString()
        {
            // Arrange
            var value = -10.5;

            // Act
            var result = value.ToCssString();

            // Assert
            Assert.Equal("-10.5", result);
        }

        [Fact]
        public void ToCssString_SmallDecimal_ReturnsCorrectString()
        {
            // Arrange
            var value = 0.123;

            // Act
            var result = value.ToCssString();

            // Assert
            Assert.Equal("0.123", result);
        }

        [Fact]
        public void ToCssString_LargeNumber_ReturnsCorrectString()
        {
            // Arrange
            var value = 1234567.89;

            // Act
            var result = value.ToCssString();

            // Assert
            Assert.Equal("1234567.89", result);
        }

        [Fact]
        public void ToCssString_GermanCulture_StillUsesDecimalPoint()
        {
            // Arrange
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            var value = 50.5;

            try
            {
                // Act
                var result = value.ToCssString();

                // Assert
                Assert.Equal("50.5", result);
                Assert.DoesNotContain(",", result);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        [Fact]
        public void ToCssString_FrenchCulture_StillUsesDecimalPoint()
        {
            // Arrange
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
            var value = 123.456;

            try
            {
                // Act
                var result = value.ToCssString();

                // Assert
                Assert.Equal("123.456", result);
                Assert.DoesNotContain(",", result);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        [Fact]
        public void ToCssString_RussianCulture_StillUsesDecimalPoint()
        {
            // Arrange
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            var value = 99.99;

            try
            {
                // Act
                var result = value.ToCssString();

                // Assert
                Assert.Equal("99.99", result);
                Assert.DoesNotContain(",", result);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }
    }
}
