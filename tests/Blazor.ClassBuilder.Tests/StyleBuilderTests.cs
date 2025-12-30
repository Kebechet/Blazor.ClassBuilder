using System.Globalization;
using System.Threading;
using Xunit;

namespace Blazor.ClassBuilder.Tests
{
    public class StyleBuilderTests
    {
        [Fact]
        public void Constructor_Empty_ReturnsEmptyString()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.Build();

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void Constructor_WithInitialValue_ReturnsStyle()
        {
            // Arrange & Act
            var builder = new StyleBuilder("color", "red");

            // Assert
            Assert.Equal("color: red;", builder.Build());
        }

        [Fact]
        public void Add_PropertyAndValue_ReturnsStyle()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.Add("color", "red").Build();

            // Assert
            Assert.Equal("color: red;", result);
        }

        [Fact]
        public void Add_MultipleStyles_ReturnsSemicolonSeparated()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder
                .Add("color", "red")
                .Add("font-size", "14px")
                .Build();

            // Assert
            Assert.Equal("color: red; font-size: 14px;", result);
        }

        [Fact]
        public void Add_RawStyle_ReturnsStyle()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.Add("color: red").Build();

            // Assert
            Assert.Equal("color: red;", result);
        }

        [Fact]
        public void Add_RawStyleWithSemicolon_DoesNotDuplicate()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.Add("color: red;").Build();

            // Assert
            Assert.Equal("color: red;", result);
        }

        [Fact]
        public void Add_DoubleValue_ReturnsStyleWithUnit()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.Add("width", 50.5, "%").Build();

            // Assert
            Assert.Equal("width: 50.5%;", result);
        }

        [Fact]
        public void Add_DoubleValue_WithoutUnit_ReturnsStyle()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.Add("opacity", 0.8).Build();

            // Assert
            Assert.Equal("opacity: 0.8;", result);
        }

        [Fact]
        public void Add_IntValue_ReturnsStyleWithUnit()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.Add("margin", 10, "px").Build();

            // Assert
            Assert.Equal("margin: 10px;", result);
        }

        [Fact]
        public void Add_IntValue_WithoutUnit_ReturnsStyle()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.Add("z-index", 10).Build();

            // Assert
            Assert.Equal("z-index: 10;", result);
        }

        [Fact]
        public void Add_DoubleValue_UsesInvariantCulture()
        {
            // Arrange
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

            try
            {
                var builder = new StyleBuilder();

                // Act
                var result = builder.Add("width", 50.5, "%").Build();

                // Assert
                Assert.Equal("width: 50.5%;", result);
                Assert.DoesNotContain(",", result);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        [Fact]
        public void Add_NullOrEmptyProperty_IsIgnored()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder
                .Add("color", "red")
                .Add("", "value")
                .Add(null, "value")
                .Add("font-size", "14px")
                .Build();

            // Assert
            Assert.Equal("color: red; font-size: 14px;", result);
        }

        [Fact]
        public void Add_NullOrEmptyValue_IsIgnored()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder
                .Add("color", "red")
                .Add("display", "")
                .Add("visibility", null)
                .Add("font-size", "14px")
                .Build();

            // Assert
            Assert.Equal("color: red; font-size: 14px;", result);
        }

        [Fact]
        public void AddIf_True_AddsStyle()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.AddIf(true, "display", "block").Build();

            // Assert
            Assert.Equal("display: block;", result);
        }

        [Fact]
        public void AddIf_False_DoesNotAddStyle()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.AddIf(false, "display", "block").Build();

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void AddIf_RawStyle_True_AddsStyle()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.AddIf(true, "display: block").Build();

            // Assert
            Assert.Equal("display: block;", result);
        }

        [Fact]
        public void AddIf_RawStyle_False_DoesNotAddStyle()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.AddIf(false, "display: block").Build();

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void AddIf_DoubleValue_True_AddsStyle()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.AddIf(true, "width", 50.5, "%").Build();

            // Assert
            Assert.Equal("width: 50.5%;", result);
        }

        [Fact]
        public void AddIf_DoubleValue_False_DoesNotAddStyle()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.AddIf(false, "width", 50.5, "%").Build();

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void AddIf_IntValue_True_AddsStyle()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.AddIf(true, "z-index", 10).Build();

            // Assert
            Assert.Equal("z-index: 10;", result);
        }

        [Fact]
        public void AddIf_IntValue_False_DoesNotAddStyle()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder.AddIf(false, "z-index", 10).Build();

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void AddIf_WithAction_True_InvokesAction()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder
                .AddIf(true, b => b.Add("color", "red"))
                .Build();

            // Assert
            Assert.Equal("color: red;", result);
        }

        [Fact]
        public void AddIf_WithAction_False_DoesNotInvokeAction()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder
                .AddIf(false, b => b.Add("color", "red"))
                .Build();

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void AddIfElse_True_AddsFirstStyle()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder
                .AddIfElse(true, "display", "block", "display", "none")
                .Build();

            // Assert
            Assert.Equal("display: block;", result);
        }

        [Fact]
        public void AddIfElse_False_AddsSecondStyle()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder
                .AddIfElse(false, "display", "block", "display", "none")
                .Build();

            // Assert
            Assert.Equal("display: none;", result);
        }

        [Fact]
        public void AddIfElse_WithActions_True_InvokesFirstAction()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder
                .AddIfElse(true, b => b.Add("color", "green"), b => b.Add("color", "red"))
                .Build();

            // Assert
            Assert.Equal("color: green;", result);
        }

        [Fact]
        public void AddIfElse_WithActions_False_InvokesSecondAction()
        {
            // Arrange
            var builder = new StyleBuilder();

            // Act
            var result = builder
                .AddIfElse(false, b => b.Add("color", "green"), b => b.Add("color", "red"))
                .Build();

            // Assert
            Assert.Equal("color: red;", result);
        }

        [Fact]
        public void Clear_RemovesAllStyles()
        {
            // Arrange
            var builder = new StyleBuilder()
                .Add("color", "red")
                .Add("font-size", "14px");

            // Act
            builder.Clear();

            // Assert
            Assert.Equal("", builder.Build());
        }

        [Fact]
        public void ToString_ReturnsSameAsBuild()
        {
            // Arrange
            var builder = new StyleBuilder()
                .Add("color", "red")
                .Add("font-size", "14px");

            // Act
            var buildResult = builder.Build();
            var toStringResult = builder.ToString();

            // Assert
            Assert.Equal(buildResult, toStringResult);
        }

        [Fact]
        public void FluentChaining_Works()
        {
            // Arrange
            var isVisible = true;
            var hasMargin = false;

            // Act
            var result = new StyleBuilder("color", "red")
                .Add("font-size", "14px")
                .AddIf(isVisible, "display", "block")
                .AddIf(hasMargin, "margin", 10, "px")
                .Add("opacity", 0.9)
                .Build();

            // Assert
            Assert.Equal("color: red; font-size: 14px; display: block; opacity: 0.9;", result);
        }
    }
}
