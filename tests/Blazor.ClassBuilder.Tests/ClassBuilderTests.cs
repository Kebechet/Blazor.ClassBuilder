using Xunit;

namespace Blazor.ClassBuilder.Tests
{
    public class ClassBuilderTests
    {
        [Fact]
        public void Constructor_Empty_ReturnsEmptyString()
        {
            var builder = new ClassBuilder();
            Assert.Equal("", builder.Build());
        }

        [Fact]
        public void Constructor_WithInitialValue_ReturnsValue()
        {
            var builder = new ClassBuilder("btn");
            Assert.Equal("btn", builder.Build());
        }

        [Fact]
        public void Add_SingleClass_ReturnsClass()
        {
            var result = new ClassBuilder()
                .Add("btn")
                .Build();

            Assert.Equal("btn", result);
        }

        [Fact]
        public void Add_MultipleClasses_ReturnsSpaceSeparated()
        {
            var result = new ClassBuilder()
                .Add("btn")
                .Add("btn-primary")
                .Build();

            Assert.Equal("btn btn-primary", result);
        }

        [Fact]
        public void Add_ParamsArray_ReturnsAllClasses()
        {
            var result = new ClassBuilder()
                .Add("btn", "btn-primary", "active")
                .Build();

            Assert.Equal("btn btn-primary active", result);
        }

        [Fact]
        public void Add_NullOrEmpty_IsIgnored()
        {
            var result = new ClassBuilder()
                .Add("btn")
                .Add("")
                .Add((string)null)
                .Add("active")
                .Build();

            Assert.Equal("btn active", result);
        }

        [Fact]
        public void AddIf_True_AddsClass()
        {
            var result = new ClassBuilder()
                .AddIf(true, "active")
                .Build();

            Assert.Equal("active", result);
        }

        [Fact]
        public void AddIf_False_DoesNotAddClass()
        {
            var result = new ClassBuilder()
                .AddIf(false, "active")
                .Build();

            Assert.Equal("", result);
        }

        [Fact]
        public void AddIf_WithAction_True_InvokesAction()
        {
            var result = new ClassBuilder()
                .AddIf(true, b => b.Add("nested"))
                .Build();

            Assert.Equal("nested", result);
        }

        [Fact]
        public void AddIf_WithAction_False_DoesNotInvokeAction()
        {
            var result = new ClassBuilder()
                .AddIf(false, b => b.Add("nested"))
                .Build();

            Assert.Equal("", result);
        }

        [Fact]
        public void AddIfElse_True_AddsFirstValue()
        {
            var result = new ClassBuilder()
                .AddIfElse(true, "show", "hide")
                .Build();

            Assert.Equal("show", result);
        }

        [Fact]
        public void AddIfElse_False_AddsSecondValue()
        {
            var result = new ClassBuilder()
                .AddIfElse(false, "show", "hide")
                .Build();

            Assert.Equal("hide", result);
        }

        [Fact]
        public void AddIfElse_WithActions_True_InvokesFirstAction()
        {
            var result = new ClassBuilder()
                .AddIfElse(true, b => b.Add("first"), b => b.Add("second"))
                .Build();

            Assert.Equal("first", result);
        }

        [Fact]
        public void AddIfElse_WithActions_False_InvokesSecondAction()
        {
            var result = new ClassBuilder()
                .AddIfElse(false, b => b.Add("first"), b => b.Add("second"))
                .Build();

            Assert.Equal("second", result);
        }

        [Fact]
        public void AddIfElse_StringAndAction_True_AddsString()
        {
            var result = new ClassBuilder()
                .AddIfElse(true, "first", b => b.Add("second"))
                .Build();

            Assert.Equal("first", result);
        }

        [Fact]
        public void AddIfElse_StringAndAction_False_InvokesAction()
        {
            var result = new ClassBuilder()
                .AddIfElse(false, "first", b => b.Add("second"))
                .Build();

            Assert.Equal("second", result);
        }

        [Fact]
        public void AddIfElse_ActionAndString_True_InvokesAction()
        {
            var result = new ClassBuilder()
                .AddIfElse(true, b => b.Add("first"), "second")
                .Build();

            Assert.Equal("first", result);
        }

        [Fact]
        public void AddIfElse_ActionAndString_False_AddsString()
        {
            var result = new ClassBuilder()
                .AddIfElse(false, b => b.Add("first"), "second")
                .Build();

            Assert.Equal("second", result);
        }

        [Fact]
        public void Clear_RemovesAllClasses()
        {
            var builder = new ClassBuilder()
                .Add("btn")
                .Add("active");

            builder.Clear();

            Assert.Equal("", builder.Build());
        }

        [Fact]
        public void ToString_ReturnsSameAsBuild()
        {
            var builder = new ClassBuilder()
                .Add("btn")
                .Add("active");

            Assert.Equal(builder.Build(), builder.ToString());
        }

        [Fact]
        public void FluentChaining_Works()
        {
            var isActive = true;
            var isDisabled = false;

            var result = new ClassBuilder("btn")
                .Add("btn-primary")
                .AddIf(isActive, "active")
                .AddIf(isDisabled, "disabled")
                .AddIfElse(isActive, "enabled", "not-enabled")
                .Build();

            Assert.Equal("btn btn-primary active enabled", result);
        }
    }
}
