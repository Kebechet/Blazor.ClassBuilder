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
            var value = 10.0;
            Assert.Equal("10", value.ToCssString());
        }

        [Fact]
        public void ToCssString_Decimal_ReturnsStringWithDot()
        {
            var value = 50.5;
            Assert.Equal("50.5", value.ToCssString());
        }

        [Fact]
        public void ToCssString_Zero_ReturnsZero()
        {
            var value = 0.0;
            Assert.Equal("0", value.ToCssString());
        }

        [Fact]
        public void ToCssString_Negative_ReturnsNegativeString()
        {
            var value = -10.5;
            Assert.Equal("-10.5", value.ToCssString());
        }

        [Fact]
        public void ToCssString_SmallDecimal_ReturnsCorrectString()
        {
            var value = 0.123;
            Assert.Equal("0.123", value.ToCssString());
        }

        [Fact]
        public void ToCssString_LargeNumber_ReturnsCorrectString()
        {
            var value = 1234567.89;
            Assert.Equal("1234567.89", value.ToCssString());
        }

        [Fact]
        public void ToCssString_GermanCulture_StillUsesDecimalPoint()
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

                var value = 50.5;
                var result = value.ToCssString();

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
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");

                var value = 123.456;
                var result = value.ToCssString();

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
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");

                var value = 99.99;
                var result = value.ToCssString();

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
