[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/kebechet)

# Blazor.ClassBuilder
[![NuGet Version](https://img.shields.io/nuget/v/Kebechet.Blazor.ClassBuilder)](https://www.nuget.org/packages/Kebechet.Blazor.ClassBuilder/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Kebechet.Blazor.ClassBuilder)](https://www.nuget.org/packages/Kebechet.Blazor.ClassBuilder/)
[![CI](https://github.com/Kebechet/Blazor.ClassBuilder/actions/workflows/ci.yml/badge.svg)](https://github.com/Kebechet/Blazor.ClassBuilder/actions/workflows/ci.yml)
[![codecov](https://codecov.io/gh/Kebechet/Blazor.ClassBuilder/graph/badge.svg)](https://codecov.io/gh/Kebechet/Blazor.ClassBuilder)
[![Twitter](https://img.shields.io/twitter/url/https/twitter.com/samuel_sidor.svg?style=social&label=Follow%20samuel_sidor)](https://x.com/samuel_sidor)

A fluent builder library for constructing CSS classes, inline styles, and HTML attributes in Blazor components.

## Features

- **ClassBuilder** - Build CSS class strings with conditional logic
- **StyleBuilder** - Build inline CSS styles with type-safe numeric values
- **AttributeBuilder** - Build HTML attribute dictionaries
- **ToCssString** - Extension for locale-safe CSS numeric values

## Installation

```bash
dotnet add package Kebechet.Blazor.ClassBuilder
```

## Quick Start

```csharp
@using Blazor.ClassBuilder
@using Blazor.ClassBuilder.Extensions

// Classes
var css = new ClassBuilder("btn")
    .AddIf(isActive, "active")
    .Build(); // "btn active"

// Styles
var style = new StyleBuilder()
    .Add("color", "red")
    .Add("width", 50.5, "%")
    .Build(); // "color: red; width: 50.5%;"

// Attributes
var attrs = new AttributeBuilder()
    .Add("type", "button")
    .AddIf(isDisabled, "disabled")
    .Build(); // Dictionary<string, object>
```

## ClassBuilder

Build CSS class strings with a fluent API:

```csharp
// Basic usage
var css = new ClassBuilder()
    .Add("btn")
    .Add("btn-primary")
    .Build(); // "btn btn-primary"

// Multiple classes at once
var css = new ClassBuilder()
    .Add("btn", "btn-primary", "rounded")
    .Build();

// Conditional classes
var css = new ClassBuilder("btn")
    .AddIf(isActive, "active")
    .AddIf(isDisabled, "disabled")
    .Build();

// If-else logic
var css = new ClassBuilder("btn")
    .AddIfElse(isPrimary, "btn-primary", "btn-secondary")
    .Build();

// Nested conditions with actions
var css = new ClassBuilder("btn")
    .AddIf(isComplex, b => b
        .Add("complex-class")
        .Add("another-class"))
    .Build();
```

### In Blazor Components

```razor
<div class="@CssClass">Content</div>

@code {
    [Parameter] public bool IsActive { get; set; }

    private string CssClass => new ClassBuilder("card")
        .AddIf(IsActive, "active")
        .Build();
}
```

## StyleBuilder

Build inline CSS styles with support for numeric values:

```csharp
// Basic usage
var style = new StyleBuilder()
    .Add("color", "red")
    .Add("font-size", "14px")
    .Build(); // "color: red; font-size: 14px;"

// Numeric values with units
var style = new StyleBuilder()
    .Add("width", 50.5, "%")      // "width: 50.5%;"
    .Add("margin", 10, "px")      // "margin: 10px;"
    .Add("opacity", 0.8)          // "opacity: 0.8;"
    .Add("z-index", 100)          // "z-index: 100;"
    .Build();

// Conditional styles
var style = new StyleBuilder()
    .Add("display", "block")
    .AddIf(isHidden, "visibility", "hidden")
    .AddIf(hasWidth, "width", width, "%")
    .Build();

// Raw style strings
var style = new StyleBuilder()
    .Add("color: red; font-weight: bold")
    .Build();
```

### Locale-Safe Numeric Values

The `StyleBuilder` automatically handles locale differences for decimal numbers. In some locales (German, French, etc.), decimals use commas (`50,5`) instead of dots (`50.5`), which breaks CSS. StyleBuilder always outputs valid CSS with decimal points.

```csharp
// Works correctly in all locales
var style = new StyleBuilder()
    .Add("width", 50.5, "%")
    .Build(); // Always "width: 50.5%;" never "width: 50,5%;"
```

### ToCssString Extension

For inline scenarios, use the `ToCssString()` extension:

```csharp
@using Blazor.ClassBuilder.Extensions

<div style="left: calc(@(percentage.ToCssString())% - 12px)">
```

### In Blazor Components

```razor
<div style="@DynamicStyle">Content</div>

@code {
    [Parameter] public double Progress { get; set; }

    private string DynamicStyle => new StyleBuilder()
        .Add("width", Progress, "%")
        .Add("background", "green")
        .Build();
}
```

## AttributeBuilder

Build HTML attribute dictionaries:

```csharp
// Basic usage
var attrs = new AttributeBuilder()
    .Add("id", "my-element")
    .Add("disabled")  // Value-less attribute
    .Build();

// Conditional attributes
var attrs = new AttributeBuilder()
    .Add("type", "text")
    .AddIf(isDisabled, "disabled")
    .AddIfFilled("placeholder", text)  // Only adds if text is not null/empty
    .Build();

// Combine builders
var baseAttrs = new AttributeBuilder().Add("class", "btn");
var attrs = new AttributeBuilder()
    .Add(baseAttrs)
    .Add("type", "submit")
    .Build();

// Validation
var attrs = new AttributeBuilder()
    .Add("value", value)
    .Throw(value < 0, "Value must be positive")
    .Build();
```

### In Blazor Components

```razor
<button @attributes="Attributes">Click me</button>

@code {
    [Parameter] public bool IsDisabled { get; set; }
    [Parameter] public string? Tooltip { get; set; }

    private Dictionary<string, object> Attributes => new AttributeBuilder()
        .Add("type", "button")
        .Add("class", "btn btn-primary")
        .AddIf(IsDisabled, "disabled")
        .AddIfFilled("title", Tooltip)
        .Build();
}
```

## Works great with `@attributes`

Merge CSS classes and inline styles from Blazor's `@attributes` / `AdditionalAttributes` dictionary:

### Merge CSS Classes

```csharp
@code {
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string CssClass => new ClassBuilder("btn")
        .Add("btn-primary")
        .AddClassFromAttributes(AdditionalAttributes)  // Merges "class" from @attributes
        .Build();
}
```

Usage in parent component:
```razor
<MyButton class="mt-2 rounded">Click me</MyButton>
<!-- Renders: class="btn btn-primary mt-2 rounded" -->
```

### Merge Inline Styles

```csharp
@code {
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string DynamicStyle => new StyleBuilder()
        .Add("display", "block")
        .AddStyleFromAttributes(AdditionalAttributes)  // Merges "style" from @attributes
        .Build();
}
```

Usage in parent component:
```razor
<MyComponent style="color: red; font-weight: bold">Content</MyComponent>
<!-- Renders: style="display: block; color: red; font-weight: bold;" -->
```

**Note:** When merging classes, tokens are deduplicated. When merging styles with duplicate properties, the builder's values come first, followed by the attribute values.

## Class Prefixing

Apply a prefix to all CSS classes for BEM or scoped design systems:

```csharp
var css = new ClassBuilder()
    .SetPrefix("sf")           // Set prefix
    .Add("btn")                // Becomes "sf-btn"
    .Add("primary")            // Becomes "sf-primary"
    .Build();                  // "sf-btn sf-primary"

// Custom separator
var css2 = new ClassBuilder()
    .SetPrefix("app", "_")     // Use underscore separator
    .Add("button")             // Becomes "app_button"
    .Build();

// Clear prefix
var css3 = new ClassBuilder()
    .SetPrefix("sf")
    .Add("btn")                // "sf-btn"
    .ClearPrefix()
    .Add("active")             // "active" (no prefix)
    .Build();                  // "sf-btn active"
```

**Note:** Prefixes apply only to classes added via `Add()` methods, NOT to classes merged from `@attributes`.

## Lazy Evaluation

Defer expensive operations until they're actually needed:

### ClassBuilder

```csharp
// Lazy condition evaluation
var css = new ClassBuilder()
    .Add(() => IsComplexCondition(), "expensive-class")
    .Build();

// Lazy value factory (only computed if condition is true)
var css2 = new ClassBuilder()
    .Add(isEnabled, () => ComputeExpensiveClassName())
    .Build();
```

### StyleBuilder

```csharp
// Lazy value factory for string values
var style = new StyleBuilder()
    .Add("background", needsGradient, () => GenerateGradient())
    .Build();

// Lazy value factory for numeric values
var style2 = new StyleBuilder()
    .Add("width", isDynamic, () => CalculateWidth(), "%")
    .Build();
```

### AttributeBuilder

```csharp
// Lazy value factory
var attrs = new AttributeBuilder()
    .Add("data-config", isComplex, () => SerializeConfig())
    .Build();
```

**Guarantee:** When the condition is false, the factory is **never invoked**.

## Avoid Rendering Empty Attributes

Use `NullIfEmpty()` to return `null` instead of an empty string, preventing Blazor from rendering empty attributes:

```csharp
<div class="@CssClass" style="@DynamicStyle">Content</div>

@code {
    private string? CssClass => new ClassBuilder()
        .AddIf(false, "hidden")
        .NullIfEmpty();  // Returns null instead of ""

    private string? DynamicStyle => new StyleBuilder()
        .AddIf(false, "display", "none")
        .NullIfEmpty();  // Returns null instead of ""
}
```

Result when conditions are false:
```html
<div>Content</div>
<!-- No class or style attributes rendered -->
```

## API Reference

### ClassBuilder

| Method | Description |
|--------|-------------|
| `Add(string)` | Add a CSS class |
| `Add(params string[])` | Add multiple CSS classes |
| `AddIf(bool, string)` | Add class if condition is true |
| `AddIf(bool, Action<ClassBuilder>)` | Execute builder action if condition is true |
| `AddIfElse(bool, string, string)` | Add first or second class based on condition |
| `AddClassFromAttributes(IReadOnlyDictionary<string, object?>?, string)` | Merge classes from attributes dictionary |
| `SetPrefix(string?, string)` | Set prefix for subsequently added classes |
| `ClearPrefix()` | Clear the prefix |
| `Add(Func<bool>, string)` | Add class with lazy condition evaluation |
| `Add(bool, Func<string>)` | Add class with lazy value factory |
| `NullIfEmpty()` | Return null if result is empty/whitespace |
| `Clear()` | Remove all classes |
| `Build()` | Get the final class string |

### StyleBuilder

| Method | Description |
|--------|-------------|
| `Add(string, string)` | Add a style property |
| `Add(string)` | Add a raw style string |
| `Add(string, double, string)` | Add numeric value with unit |
| `Add(string, int, string)` | Add integer value with unit |
| `AddIf(bool, ...)` | Add style if condition is true |
| `AddIfElse(bool, ...)` | Add style based on condition |
| `AddStyleFromAttributes(IReadOnlyDictionary<string, object?>?, string)` | Merge styles from attributes dictionary |
| `AddVerbatim(string)` | Add raw CSS snippet verbatim |
| `Add(Func<bool>, string, string)` | Add style with lazy condition evaluation |
| `Add(string, bool, Func<string>)` | Add style with lazy string value factory |
| `Add(string, bool, Func<double>, string)` | Add style with lazy numeric value factory |
| `NullIfEmpty()` | Return null if result is empty/whitespace |
| `Clear()` | Remove all styles |
| `Build()` | Get the final style string |

### AttributeBuilder

| Method | Description |
|--------|-------------|
| `Add(string, object?)` | Add an attribute |
| `Add(AttributeBuilder)` | Merge another builder |
| `AddIf(bool, string, object?)` | Add if condition is true |
| `AddIfFilled(string, object?)` | Add if value is not null/empty |
| `Add(string, bool, Func<object?>)` | Add attribute with lazy value factory |
| `Throw(bool, string)` | Throw exception if condition is true |
| `Build()` | Get the attribute dictionary |

## License

This project is licensed under the [MIT](LICENSE) license.
