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
            var builder = new StyleBuilder();
            Assert.Equal("", builder.Build());
        }

        [Fact]
        public void Constructor_WithInitialValue_ReturnsStyle()
        {
            var builder = new StyleBuilder("color", "red");
            Assert.Equal("color: red;", builder.Build());
        }

        [Fact]
        public void Add_PropertyAndValue_ReturnsStyle()
        {
            var result = new StyleBuilder()
                .Add("color", "red")
                .Build();

            Assert.Equal("color: red;", result);
        }

        [Fact]
        public void Add_MultipleStyles_ReturnsSemicolonSeparated()
        {
            var result = new StyleBuilder()
                .Add("color", "red")
                .Add("font-size", "14px")
                .Build();

            Assert.Equal("color: red; font-size: 14px;", result);
        }

        [Fact]
        public void Add_RawStyle_ReturnsStyle()
        {
            var result = new StyleBuilder()
                .Add("color: red")
                .Build();

            Assert.Equal("color: red;", result);
        }

        [Fact]
        public void Add_RawStyleWithSemicolon_DoesNotDuplicate()
        {
            var result = new StyleBuilder()
                .Add("color: red;")
                .Build();

            Assert.Equal("color: red;", result);
        }

        [Fact]
        public void Add_DoubleValue_ReturnsStyleWithUnit()
        {
            var result = new StyleBuilder()
                .Add("width", 50.5, "%")
                .Build();

            Assert.Equal("width: 50.5%;", result);
        }

        [Fact]
        public void Add_DoubleValue_WithoutUnit_ReturnsStyle()
        {
            var result = new StyleBuilder()
                .Add("opacity", 0.8)
                .Build();

            Assert.Equal("opacity: 0.8;", result);
        }

        [Fact]
        public void Add_IntValue_ReturnsStyleWithUnit()
        {
            var result = new StyleBuilder()
                .Add("margin", 10, "px")
                .Build();

            Assert.Equal("margin: 10px;", result);
        }

        [Fact]
        public void Add_IntValue_WithoutUnit_ReturnsStyle()
        {
            var result = new StyleBuilder()
                .Add("z-index", 10)
                .Build();

            Assert.Equal("z-index: 10;", result);
        }

        [Fact]
        public void Add_DoubleValue_UsesInvariantCulture()
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

                var result = new StyleBuilder()
                    .Add("width", 50.5, "%")
                    .Build();

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
            var result = new StyleBuilder()
                .Add("color", "red")
                .Add("", "value")
                .Add(null, "value")
                .Add("font-size", "14px")
                .Build();

            Assert.Equal("color: red; font-size: 14px;", result);
        }

        [Fact]
        public void Add_NullOrEmptyValue_IsIgnored()
        {
            var result = new StyleBuilder()
                .Add("color", "red")
                .Add("display", "")
                .Add("visibility", null)
                .Add("font-size", "14px")
                .Build();

            Assert.Equal("color: red; font-size: 14px;", result);
        }

        [Fact]
        public void AddIf_True_AddsStyle()
        {
            var result = new StyleBuilder()
                .AddIf(true, "display", "block")
                .Build();

            Assert.Equal("display: block;", result);
        }

        [Fact]
        public void AddIf_False_DoesNotAddStyle()
        {
            var result = new StyleBuilder()
                .AddIf(false, "display", "block")
                .Build();

            Assert.Equal("", result);
        }

        [Fact]
        public void AddIf_RawStyle_True_AddsStyle()
        {
            var result = new StyleBuilder()
                .AddIf(true, "display: block")
                .Build();

            Assert.Equal("display: block;", result);
        }

        [Fact]
        public void AddIf_RawStyle_False_DoesNotAddStyle()
        {
            var result = new StyleBuilder()
                .AddIf(false, "display: block")
                .Build();

            Assert.Equal("", result);
        }

        [Fact]
        public void AddIf_DoubleValue_True_AddsStyle()
        {
            var result = new StyleBuilder()
                .AddIf(true, "width", 50.5, "%")
                .Build();

            Assert.Equal("width: 50.5%;", result);
        }

        [Fact]
        public void AddIf_DoubleValue_False_DoesNotAddStyle()
        {
            var result = new StyleBuilder()
                .AddIf(false, "width", 50.5, "%")
                .Build();

            Assert.Equal("", result);
        }

        [Fact]
        public void AddIf_IntValue_True_AddsStyle()
        {
            var result = new StyleBuilder()
                .AddIf(true, "z-index", 10)
                .Build();

            Assert.Equal("z-index: 10;", result);
        }

        [Fact]
        public void AddIf_IntValue_False_DoesNotAddStyle()
        {
            var result = new StyleBuilder()
                .AddIf(false, "z-index", 10)
                .Build();

            Assert.Equal("", result);
        }

        [Fact]
        public void AddIf_WithAction_True_InvokesAction()
        {
            var result = new StyleBuilder()
                .AddIf(true, b => b.Add("color", "red"))
                .Build();

            Assert.Equal("color: red;", result);
        }

        [Fact]
        public void AddIf_WithAction_False_DoesNotInvokeAction()
        {
            var result = new StyleBuilder()
                .AddIf(false, b => b.Add("color", "red"))
                .Build();

            Assert.Equal("", result);
        }

        [Fact]
        public void AddIfElse_True_AddsFirstStyle()
        {
            var result = new StyleBuilder()
                .AddIfElse(true, "display", "block", "display", "none")
                .Build();

            Assert.Equal("display: block;", result);
        }

        [Fact]
        public void AddIfElse_False_AddsSecondStyle()
        {
            var result = new StyleBuilder()
                .AddIfElse(false, "display", "block", "display", "none")
                .Build();

            Assert.Equal("display: none;", result);
        }

        [Fact]
        public void AddIfElse_WithActions_True_InvokesFirstAction()
        {
            var result = new StyleBuilder()
                .AddIfElse(true, b => b.Add("color", "green"), b => b.Add("color", "red"))
                .Build();

            Assert.Equal("color: green;", result);
        }

        [Fact]
        public void AddIfElse_WithActions_False_InvokesSecondAction()
        {
            var result = new StyleBuilder()
                .AddIfElse(false, b => b.Add("color", "green"), b => b.Add("color", "red"))
                .Build();

            Assert.Equal("color: red;", result);
        }

        [Fact]
        public void Clear_RemovesAllStyles()
        {
            var builder = new StyleBuilder()
                .Add("color", "red")
                .Add("font-size", "14px");

            builder.Clear();

            Assert.Equal("", builder.Build());
        }

        [Fact]
        public void ToString_ReturnsSameAsBuild()
        {
            var builder = new StyleBuilder()
                .Add("color", "red")
                .Add("font-size", "14px");

            Assert.Equal(builder.Build(), builder.ToString());
        }

        [Fact]
        public void FluentChaining_Works()
        {
            var isVisible = true;
            var hasMargin = false;

            var result = new StyleBuilder("color", "red")
                .Add("font-size", "14px")
                .AddIf(isVisible, "display", "block")
                .AddIf(hasMargin, "margin", 10, "px")
                .Add("opacity", 0.9)
                .Build();

            Assert.Equal("color: red; font-size: 14px; display: block; opacity: 0.9;", result);
        }
    }
}
