using Xunit;

namespace Blazor.ClassBuilder.Tests
{
    public class ClassBuilderTests
    {
        [Fact]
        public void Constructor_Empty_ReturnsEmptyString()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder.Build();

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void Constructor_WithInitialValue_ReturnsValue()
        {
            // Arrange & Act
            var builder = new ClassBuilder("btn");

            // Assert
            Assert.Equal("btn", builder.Build());
        }

        [Fact]
        public void Add_SingleClass_ReturnsClass()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder.Add("btn").Build();

            // Assert
            Assert.Equal("btn", result);
        }

        [Fact]
        public void Add_MultipleClasses_ReturnsSpaceSeparated()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder
                .Add("btn")
                .Add("btn-primary")
                .Build();

            // Assert
            Assert.Equal("btn btn-primary", result);
        }

        [Fact]
        public void Add_ParamsArray_ReturnsAllClasses()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder
                .Add("btn", "btn-primary", "active")
                .Build();

            // Assert
            Assert.Equal("btn btn-primary active", result);
        }

        [Fact]
        public void Add_NullOrEmpty_IsIgnored()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder
                .Add("btn")
                .Add("")
                .Add((string)null)
                .Add("active")
                .Build();

            // Assert
            Assert.Equal("btn active", result);
        }

        [Fact]
        public void AddIf_True_AddsClass()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder
                .AddIf(true, "active")
                .Build();

            // Assert
            Assert.Equal("active", result);
        }

        [Fact]
        public void AddIf_False_DoesNotAddClass()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder
                .AddIf(false, "active")
                .Build();

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void AddIf_WithAction_True_InvokesAction()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder
                .AddIf(true, b => b.Add("nested"))
                .Build();

            // Assert
            Assert.Equal("nested", result);
        }

        [Fact]
        public void AddIf_WithAction_False_DoesNotInvokeAction()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder
                .AddIf(false, b => b.Add("nested"))
                .Build();

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void AddIfElse_True_AddsFirstValue()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder
                .AddIfElse(true, "show", "hide")
                .Build();

            // Assert
            Assert.Equal("show", result);
        }

        [Fact]
        public void AddIfElse_False_AddsSecondValue()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder
                .AddIfElse(false, "show", "hide")
                .Build();

            // Assert
            Assert.Equal("hide", result);
        }

        [Fact]
        public void AddIfElse_WithActions_True_InvokesFirstAction()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder
                .AddIfElse(true, b => b.Add("first"), b => b.Add("second"))
                .Build();

            // Assert
            Assert.Equal("first", result);
        }

        [Fact]
        public void AddIfElse_WithActions_False_InvokesSecondAction()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder
                .AddIfElse(false, b => b.Add("first"), b => b.Add("second"))
                .Build();

            // Assert
            Assert.Equal("second", result);
        }

        [Fact]
        public void AddIfElse_StringAndAction_True_AddsString()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder
                .AddIfElse(true, "first", b => b.Add("second"))
                .Build();

            // Assert
            Assert.Equal("first", result);
        }

        [Fact]
        public void AddIfElse_StringAndAction_False_InvokesAction()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder
                .AddIfElse(false, "first", b => b.Add("second"))
                .Build();

            // Assert
            Assert.Equal("second", result);
        }

        [Fact]
        public void AddIfElse_ActionAndString_True_InvokesAction()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder
                .AddIfElse(true, b => b.Add("first"), "second")
                .Build();

            // Assert
            Assert.Equal("first", result);
        }

        [Fact]
        public void AddIfElse_ActionAndString_False_AddsString()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder
                .AddIfElse(false, b => b.Add("first"), "second")
                .Build();

            // Assert
            Assert.Equal("second", result);
        }

        [Fact]
        public void Clear_RemovesAllClasses()
        {
            // Arrange
            var builder = new ClassBuilder()
                .Add("btn")
                .Add("active");

            // Act
            builder.Clear();

            // Assert
            Assert.Equal("", builder.Build());
        }

        [Fact]
        public void ToString_ReturnsSameAsBuild()
        {
            // Arrange
            var builder = new ClassBuilder()
                .Add("btn")
                .Add("active");

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
            var isActive = true;
            var isDisabled = false;

            // Act
            var result = new ClassBuilder("btn")
                .Add("btn-primary")
                .AddIf(isActive, "active")
                .AddIf(isDisabled, "disabled")
                .AddIfElse(isActive, "enabled", "not-enabled")
                .Build();

            // Assert
            Assert.Equal("btn btn-primary active enabled", result);
        }
    }
}
