using System.Collections.Generic;
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

        // Feature 1: AddClassFromAttributes tests
        [Fact]
        public void AddClassFromAttributes_NullAttributes_NoOp()
        {
            // Arrange
            var builder = new ClassBuilder("btn");

            // Act
            var result = builder.AddClassFromAttributes(null).Build();

            // Assert
            Assert.Equal("btn", result);
        }

        [Fact]
        public void AddClassFromAttributes_MissingKey_NoOp()
        {
            // Arrange
            var builder = new ClassBuilder("btn");
            var attrs = new Dictionary<string, object?> { { "id", "test" } };

            // Act
            var result = builder.AddClassFromAttributes(attrs).Build();

            // Assert
            Assert.Equal("btn", result);
        }

        [Fact]
        public void AddClassFromAttributes_EmptyValue_NoOp()
        {
            // Arrange
            var builder = new ClassBuilder("btn");
            var attrs = new Dictionary<string, object?> { { "class", "" } };

            // Act
            var result = builder.AddClassFromAttributes(attrs).Build();

            // Assert
            Assert.Equal("btn", result);
        }

        [Fact]
        public void AddClassFromAttributes_WhitespaceValue_NoOp()
        {
            // Arrange
            var builder = new ClassBuilder("btn");
            var attrs = new Dictionary<string, object?> { { "class", "   " } };

            // Act
            var result = builder.AddClassFromAttributes(attrs).Build();

            // Assert
            Assert.Equal("btn", result);
        }

        [Fact]
        public void AddClassFromAttributes_NullValue_NoOp()
        {
            // Arrange
            var builder = new ClassBuilder("btn");
            var attrs = new Dictionary<string, object?> { { "class", null } };

            // Act
            var result = builder.AddClassFromAttributes(attrs).Build();

            // Assert
            Assert.Equal("btn", result);
        }

        [Fact]
        public void AddClassFromAttributes_ValidValue_AddsClasses()
        {
            // Arrange
            var builder = new ClassBuilder("btn");
            var attrs = new Dictionary<string, object?> { { "class", "active primary" } };

            // Act
            var result = builder.AddClassFromAttributes(attrs).Build();

            // Assert
            Assert.Equal("btn active primary", result);
        }

        [Fact]
        public void AddClassFromAttributes_MultipleWhitespaces_NormalizesTokens()
        {
            // Arrange
            var builder = new ClassBuilder("btn");
            var attrs = new Dictionary<string, object?> { { "class", "  active   primary  \t rounded  " } };

            // Act
            var result = builder.AddClassFromAttributes(attrs).Build();

            // Assert
            Assert.Equal("btn active primary rounded", result);
        }

        [Fact]
        public void AddClassFromAttributes_Deduplicates()
        {
            // Arrange
            var builder = new ClassBuilder();
            var attrs = new Dictionary<string, object?> { { "class", "btn btn active btn" } };

            // Act
            var result = builder.AddClassFromAttributes(attrs).Build();

            // Assert
            Assert.Equal("btn active", result);
        }

        [Fact]
        public void AddClassFromAttributes_ObjectValue_ConvertsToString()
        {
            // Arrange
            var builder = new ClassBuilder("btn");
            var attrs = new Dictionary<string, object?> { { "class", 123 } };

            // Act
            var result = builder.AddClassFromAttributes(attrs).Build();

            // Assert
            Assert.Equal("btn 123", result);
        }

        [Fact]
        public void AddClassFromAttributes_CustomKey_Works()
        {
            // Arrange
            var builder = new ClassBuilder("btn");
            var attrs = new Dictionary<string, object?> { { "customClass", "active primary" } };

            // Act
            var result = builder.AddClassFromAttributes(attrs, "customClass").Build();

            // Assert
            Assert.Equal("btn active primary", result);
        }

        // Feature 3: Prefix tests
        [Fact]
        public void SetPrefix_AppliesPrefixToSubsequentClasses()
        {
            // Arrange & Act
            var result = new ClassBuilder()
                .SetPrefix("sf")
                .Add("btn")
                .Add("primary")
                .Build();

            // Assert
            Assert.Equal("sf-btn sf-primary", result);
        }

        [Fact]
        public void SetPrefix_WithCustomSeparator_UsesSeparator()
        {
            // Arrange & Act
            var result = new ClassBuilder()
                .SetPrefix("sf", "_")
                .Add("btn")
                .Build();

            // Assert
            Assert.Equal("sf_btn", result);
        }

        [Fact]
        public void SetPrefix_NullPrefix_ClearsPrefix()
        {
            // Arrange & Act
            var result = new ClassBuilder()
                .SetPrefix("sf")
                .Add("btn")
                .SetPrefix(null)
                .Add("primary")
                .Build();

            // Assert
            Assert.Equal("sf-btn primary", result);
        }

        [Fact]
        public void ClearPrefix_RemovesPrefix()
        {
            // Arrange & Act
            var result = new ClassBuilder()
                .SetPrefix("sf")
                .Add("btn")
                .ClearPrefix()
                .Add("primary")
                .Build();

            // Assert
            Assert.Equal("sf-btn primary", result);
        }

        [Fact]
        public void SetPrefix_DoesNotApplyToAttributeClasses()
        {
            // Arrange
            var attrs = new Dictionary<string, object?> { { "class", "active rounded" } };

            // Act
            var result = new ClassBuilder()
                .SetPrefix("sf")
                .Add("btn")
                .AddClassFromAttributes(attrs)
                .Add("primary")
                .Build();

            // Assert
            Assert.Equal("sf-btn active rounded sf-primary", result);
        }

        [Fact]
        public void SetPrefix_MultiTokenAdd_PrefixesEachToken()
        {
            // Arrange & Act
            var result = new ClassBuilder()
                .SetPrefix("sf")
                .Add("btn", "primary", "large")
                .Build();

            // Assert
            Assert.Equal("sf-btn sf-primary sf-large", result);
        }

        // Feature 4: NullIfEmpty tests
        [Fact]
        public void NullIfEmpty_EmptyBuilder_ReturnsNull()
        {
            // Arrange
            var builder = new ClassBuilder();

            // Act
            var result = builder.NullIfEmpty();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void NullIfEmpty_OnlyWhitespace_ReturnsNull()
        {
            // Arrange
            var builder = new ClassBuilder()
                .AddIf(false, "btn");

            // Act
            var result = builder.NullIfEmpty();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void NullIfEmpty_WithContent_ReturnsString()
        {
            // Arrange
            var builder = new ClassBuilder("btn");

            // Act
            var result = builder.NullIfEmpty();

            // Assert
            Assert.Equal("btn", result);
        }

        // Feature 5: Lazy evaluation tests
        [Fact]
        public void Add_LazyCondition_EvaluatesCondition()
        {
            // Arrange
            var conditionCalled = false;
            bool Condition() { conditionCalled = true; return true; }

            // Act
            var result = new ClassBuilder()
                .Add(Condition, "btn")
                .Build();

            // Assert
            Assert.True(conditionCalled);
            Assert.Equal("btn", result);
        }

        [Fact]
        public void Add_LazyCondition_False_DoesNotAddClass()
        {
            // Arrange
            var conditionCalled = false;
            bool Condition() { conditionCalled = true; return false; }

            // Act
            var result = new ClassBuilder()
                .Add(Condition, "btn")
                .Build();

            // Assert
            Assert.True(conditionCalled);
            Assert.Equal("", result);
        }

        [Fact]
        public void Add_LazyValue_True_InvokesFactory()
        {
            // Arrange
            var factoryCalled = false;
            string Factory() { factoryCalled = true; return "btn"; }

            // Act
            var result = new ClassBuilder()
                .Add(true, Factory)
                .Build();

            // Assert
            Assert.True(factoryCalled);
            Assert.Equal("btn", result);
        }

        [Fact]
        public void Add_LazyValue_False_DoesNotInvokeFactory()
        {
            // Arrange
            var factoryCalled = false;
            string Factory() { factoryCalled = true; return "btn"; }

            // Act
            var result = new ClassBuilder()
                .Add(false, Factory)
                .Build();

            // Assert
            Assert.False(factoryCalled);
            Assert.Equal("", result);
        }
    }
}
