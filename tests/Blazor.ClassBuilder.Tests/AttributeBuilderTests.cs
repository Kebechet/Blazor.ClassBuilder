using System;
using Xunit;

namespace Blazor.ClassBuilder.Tests
{
    public class AttributeBuilderTests
    {
        [Fact]
        public void Build_Empty_ReturnsEmptyDictionary()
        {
            var builder = new AttributeBuilder();
            var result = builder.Build();

            Assert.Empty(result);
        }

        [Fact]
        public void Add_NameOnly_AddsAttributeWithEmptyValue()
        {
            var result = new AttributeBuilder()
                .Add("disabled")
                .Build();

            Assert.Single(result);
            Assert.True(result.ContainsKey("disabled"));
            Assert.Equal("", result["disabled"]);
        }

        [Fact]
        public void Add_NameAndValue_AddsAttribute()
        {
            var result = new AttributeBuilder()
                .Add("placeholder", "Enter your name")
                .Build();

            Assert.Single(result);
            Assert.Equal("Enter your name", result["placeholder"]);
        }

        [Fact]
        public void Add_MultipleAttributes_AddsAll()
        {
            var result = new AttributeBuilder()
                .Add("type", "text")
                .Add("placeholder", "Enter name")
                .Add("maxlength", 50)
                .Build();

            Assert.Equal(3, result.Count);
            Assert.Equal("text", result["type"]);
            Assert.Equal("Enter name", result["placeholder"]);
            Assert.Equal("50", result["maxlength"]);
        }

        [Fact]
        public void Add_AttributeBuilder_MergesAttributes()
        {
            var other = new AttributeBuilder()
                .Add("class", "btn")
                .Add("id", "submit-btn");

            var result = new AttributeBuilder()
                .Add("type", "button")
                .Add(other)
                .Build();

            Assert.Equal(3, result.Count);
            Assert.Equal("button", result["type"]);
            Assert.Equal("btn", result["class"]);
            Assert.Equal("submit-btn", result["id"]);
        }

        [Fact]
        public void Add_NullAttributeBuilder_IsIgnored()
        {
            var result = new AttributeBuilder()
                .Add("type", "button")
                .Add((AttributeBuilder)null)
                .Build();

            Assert.Single(result);
            Assert.Equal("button", result["type"]);
        }

        [Fact]
        public void Add_EmptyAttributeBuilder_IsIgnored()
        {
            var empty = new AttributeBuilder();

            var result = new AttributeBuilder()
                .Add("type", "button")
                .Add(empty)
                .Build();

            Assert.Single(result);
            Assert.Equal("button", result["type"]);
        }

        [Fact]
        public void AddIf_True_AddsAttribute()
        {
            var result = new AttributeBuilder()
                .AddIf(true, "disabled")
                .Build();

            Assert.Single(result);
            Assert.True(result.ContainsKey("disabled"));
        }

        [Fact]
        public void AddIf_False_DoesNotAddAttribute()
        {
            var result = new AttributeBuilder()
                .AddIf(false, "disabled")
                .Build();

            Assert.Empty(result);
        }

        [Fact]
        public void AddIf_WithValue_True_AddsAttribute()
        {
            var result = new AttributeBuilder()
                .AddIf(true, "placeholder", "Enter name")
                .Build();

            Assert.Single(result);
            Assert.Equal("Enter name", result["placeholder"]);
        }

        [Fact]
        public void AddIf_WithValue_False_DoesNotAddAttribute()
        {
            var result = new AttributeBuilder()
                .AddIf(false, "placeholder", "Enter name")
                .Build();

            Assert.Empty(result);
        }

        [Fact]
        public void AddIfFilled_WithValue_AddsAttribute()
        {
            var result = new AttributeBuilder()
                .AddIfFilled("placeholder", "Enter name")
                .Build();

            Assert.Single(result);
            Assert.Equal("Enter name", result["placeholder"]);
        }

        [Fact]
        public void AddIfFilled_WithNull_DoesNotAddAttribute()
        {
            var result = new AttributeBuilder()
                .AddIfFilled("placeholder", null)
                .Build();

            Assert.Empty(result);
        }

        [Fact]
        public void AddIfFilled_WithEmptyString_DoesNotAddAttribute()
        {
            var result = new AttributeBuilder()
                .AddIfFilled("placeholder", "")
                .Build();

            Assert.Empty(result);
        }

        [Fact]
        public void Throw_False_DoesNotThrow()
        {
            var builder = new AttributeBuilder()
                .Throw(false, "Should not throw");

            Assert.NotNull(builder);
        }

        [Fact]
        public void Throw_True_ThrowsArgumentException()
        {
            var builder = new AttributeBuilder();

            var exception = Assert.Throws<ArgumentException>(() =>
                builder.Throw(true, "Test exception message"));

            Assert.Equal("Test exception message", exception.Message);
        }

        [Fact]
        public void FluentChaining_Works()
        {
            var isDisabled = true;
            var placeholder = "Enter name";

            var result = new AttributeBuilder()
                .Add("type", "text")
                .AddIf(isDisabled, "disabled")
                .AddIfFilled("placeholder", placeholder)
                .AddIf(false, "readonly")
                .Build();

            Assert.Equal(3, result.Count);
            Assert.Equal("text", result["type"]);
            Assert.True(result.ContainsKey("disabled"));
            Assert.Equal("Enter name", result["placeholder"]);
            Assert.False(result.ContainsKey("readonly"));
        }

        [Fact]
        public void Add_NullValue_AddsEmptyString()
        {
            var result = new AttributeBuilder()
                .Add("data-attr", null)
                .Build();

            Assert.Single(result);
            Assert.Equal("", result["data-attr"]);
        }

        [Fact]
        public void Add_IntValue_ConvertsToString()
        {
            var result = new AttributeBuilder()
                .Add("tabindex", 1)
                .Build();

            Assert.Equal("1", result["tabindex"]);
        }

        [Fact]
        public void Add_BoolValue_ConvertsToString()
        {
            var result = new AttributeBuilder()
                .Add("data-active", true)
                .Build();

            Assert.Equal("True", result["data-active"]);
        }
    }
}
