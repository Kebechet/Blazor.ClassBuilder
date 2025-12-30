using System;
using Xunit;

namespace Blazor.ClassBuilder.Tests
{
    public class AttributeBuilderTests
    {
        [Fact]
        public void Build_Empty_ReturnsEmptyDictionary()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act
            var result = builder.Build();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Add_NameOnly_AddsAttributeWithEmptyValue()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act
            var result = builder.Add("disabled").Build();

            // Assert
            Assert.Single(result);
            Assert.True(result.ContainsKey("disabled"));
            Assert.Equal("", result["disabled"]);
        }

        [Fact]
        public void Add_NameAndValue_AddsAttribute()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act
            var result = builder.Add("placeholder", "Enter your name").Build();

            // Assert
            Assert.Single(result);
            Assert.Equal("Enter your name", result["placeholder"]);
        }

        [Fact]
        public void Add_MultipleAttributes_AddsAll()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act
            var result = builder
                .Add("type", "text")
                .Add("placeholder", "Enter name")
                .Add("maxlength", 50)
                .Build();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("text", result["type"]);
            Assert.Equal("Enter name", result["placeholder"]);
            Assert.Equal("50", result["maxlength"]);
        }

        [Fact]
        public void Add_AttributeBuilder_MergesAttributes()
        {
            // Arrange
            var other = new AttributeBuilder()
                .Add("class", "btn")
                .Add("id", "submit-btn");
            var builder = new AttributeBuilder().Add("type", "button");

            // Act
            var result = builder.Add(other).Build();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("button", result["type"]);
            Assert.Equal("btn", result["class"]);
            Assert.Equal("submit-btn", result["id"]);
        }

        [Fact]
        public void Add_NullAttributeBuilder_IsIgnored()
        {
            // Arrange
            var builder = new AttributeBuilder().Add("type", "button");

            // Act
            var result = builder.Add((AttributeBuilder)null).Build();

            // Assert
            Assert.Single(result);
            Assert.Equal("button", result["type"]);
        }

        [Fact]
        public void Add_EmptyAttributeBuilder_IsIgnored()
        {
            // Arrange
            var empty = new AttributeBuilder();
            var builder = new AttributeBuilder().Add("type", "button");

            // Act
            var result = builder.Add(empty).Build();

            // Assert
            Assert.Single(result);
            Assert.Equal("button", result["type"]);
        }

        [Fact]
        public void AddIf_True_AddsAttribute()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act
            var result = builder.AddIf(true, "disabled").Build();

            // Assert
            Assert.Single(result);
            Assert.True(result.ContainsKey("disabled"));
        }

        [Fact]
        public void AddIf_False_DoesNotAddAttribute()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act
            var result = builder.AddIf(false, "disabled").Build();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void AddIf_WithValue_True_AddsAttribute()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act
            var result = builder.AddIf(true, "placeholder", "Enter name").Build();

            // Assert
            Assert.Single(result);
            Assert.Equal("Enter name", result["placeholder"]);
        }

        [Fact]
        public void AddIf_WithValue_False_DoesNotAddAttribute()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act
            var result = builder.AddIf(false, "placeholder", "Enter name").Build();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void AddIfFilled_WithValue_AddsAttribute()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act
            var result = builder.AddIfFilled("placeholder", "Enter name").Build();

            // Assert
            Assert.Single(result);
            Assert.Equal("Enter name", result["placeholder"]);
        }

        [Fact]
        public void AddIfFilled_WithNull_DoesNotAddAttribute()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act
            var result = builder.AddIfFilled("placeholder", null).Build();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void AddIfFilled_WithEmptyString_DoesNotAddAttribute()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act
            var result = builder.AddIfFilled("placeholder", "").Build();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Throw_False_DoesNotThrow()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act
            var result = builder.Throw(false, "Should not throw");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Throw_True_ThrowsArgumentException()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                builder.Throw(true, "Test exception message"));
            Assert.Equal("Test exception message", exception.Message);
        }

        [Fact]
        public void FluentChaining_Works()
        {
            // Arrange
            var isDisabled = true;
            var placeholder = "Enter name";

            // Act
            var result = new AttributeBuilder()
                .Add("type", "text")
                .AddIf(isDisabled, "disabled")
                .AddIfFilled("placeholder", placeholder)
                .AddIf(false, "readonly")
                .Build();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("text", result["type"]);
            Assert.True(result.ContainsKey("disabled"));
            Assert.Equal("Enter name", result["placeholder"]);
            Assert.False(result.ContainsKey("readonly"));
        }

        [Fact]
        public void Add_NullValue_AddsEmptyString()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act
            var result = builder.Add("data-attr", null).Build();

            // Assert
            Assert.Single(result);
            Assert.Equal("", result["data-attr"]);
        }

        [Fact]
        public void Add_IntValue_ConvertsToString()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act
            var result = builder.Add("tabindex", 1).Build();

            // Assert
            Assert.Equal("1", result["tabindex"]);
        }

        [Fact]
        public void Add_BoolValue_ConvertsToString()
        {
            // Arrange
            var builder = new AttributeBuilder();

            // Act
            var result = builder.Add("data-active", true).Build();

            // Assert
            Assert.Equal("True", result["data-active"]);
        }

        // Feature 5: Lazy evaluation tests
        [Fact]
        public void Add_LazyValue_True_InvokesFactory()
        {
            // Arrange
            var factoryCalled = false;
            object Factory() { factoryCalled = true; return "test-value"; }

            // Act
            var result = new AttributeBuilder()
                .Add(true, "data-value", Factory)
                .Build();

            // Assert
            Assert.True(factoryCalled);
            Assert.Equal("test-value", result["data-value"]);
        }

        [Fact]
        public void Add_LazyValue_False_DoesNotInvokeFactory()
        {
            // Arrange
            var factoryCalled = false;
            object Factory() { factoryCalled = true; return "test-value"; }

            // Act
            var result = new AttributeBuilder()
                .Add(false, "data-value", Factory)
                .Build();

            // Assert
            Assert.False(factoryCalled);
            Assert.False(result.ContainsKey("data-value"));
        }
    }
}
