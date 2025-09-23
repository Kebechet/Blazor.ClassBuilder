[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/kebechet)

# Blazor.ClassBuilder
[![NuGet Version](https://img.shields.io/nuget/v/Kebechet.Blazor.ClassBuilder)](https://www.nuget.org/packages/Kebechet.Blazor.ClassBuilder/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Kebechet.Blazor.ClassBuilder)](https://www.nuget.org/packages/Kebechet.Blazor.ClassBuilder/)
[![Twitter](https://img.shields.io/twitter/url/https/twitter.com/samuel_sidor.svg?style=social&label=Follow%20samuel_sidor)](https://x.com/samuel_sidor)

A fluent builder library for constructing CSS class names and HTML attributes in Blazor components. This library provides a clean and intuitive way to conditionally apply CSS classes and HTML attributes in your Blazor applications.

## Installation

Install the package via NuGet Package Manager:

```bash
dotnet add package Kebechet.Blazor.ClassBuilder
```

Or via Package Manager Console:

```powershell
Install-Package Kebechet.Blazor.ClassBuilder
```

## Usage

### ClassBuilder

The `ClassBuilder` provides a fluent API for building CSS class strings:

#### Basic Usage

```csharp
using Blazor.ClassBuilder;

// Simple class building
var cssClass = new ClassBuilder()
    .Add("btn")
    .Add("btn-primary")
    .Build(); // Returns: "btn btn-primary"

// With initial value
var cssClass = new ClassBuilder("btn")
    .Add("btn-primary")
    .Build(); // Returns: "btn btn-primary"
```

#### Conditional Classes

```csharp
var isActive = true;
var isDisabled = false;

var cssClass = new ClassBuilder("btn")
    .AddIf(isActive, "active")
    .AddIf(isDisabled, "disabled")
    .Build(); // Returns: "btn active"
```

#### Conditional with Actions

```csharp
var cssClass = new ClassBuilder("btn")
    .AddIf(someCondition, builder => builder
        .Add("complex-class")
        .Add("another-class")
    )
    .Build();
```

#### If-Else Logic

```csharp
var cssClass = new ClassBuilder("btn")
    .AddIfElse(isPrimary, "btn-primary", "btn-secondary")
    .Build();

// With actions
var cssClass = new ClassBuilder("btn")
    .AddIfElse(isComplex, 
        builder => builder.Add("complex").Add("primary"),
        builder => builder.Add("simple").Add("secondary")
    )
    .Build();
```

#### In Blazor Components

```razor
@using Blazor.ClassBuilder

<div class="@CssClass">
    Content here
</div>

@code {
    [Parameter] public bool IsActive { get; set; }
    [Parameter] public bool IsDisabled { get; set; }
    [Parameter] public string? Size { get; set; }

    private string CssClass => new ClassBuilder("component")
        .AddIf(IsActive, "active")
        .AddIf(IsDisabled, "disabled")
        .AddIf(!string.IsNullOrEmpty(Size), $"size-{Size}")
        .Build();
}
```

### AttributeBuilder

The `AttributeBuilder` provides a fluent API for building HTML attribute dictionaries:

#### Basic Usage

```csharp
using Blazor.ClassBuilder;

var attributes = new AttributeBuilder()
    .Add("id", "my-element")
    .Add("disabled")
    .Build(); // Returns: Dictionary with "id" -> "my-element", "disabled" -> ""
```

#### Conditional Attributes

```csharp
var attributes = new AttributeBuilder()
    .Add("type", "button")
    .AddIf(isDisabled, "disabled")
    .AddIfFilled("placeholder", placeholderText) // Only adds if placeholderText is not null/empty
    .Build();
```

#### Combining AttributeBuilders

```csharp
var baseAttributes = new AttributeBuilder()
    .Add("class", "btn")
    .Add("type", "button");

var finalAttributes = new AttributeBuilder()
    .Add(baseAttributes)
    .AddIf(isDisabled, "disabled")
    .Build();
```

#### Exception Handling

```csharp
var attributes = new AttributeBuilder()
    .Add("type", "button")
    .Throw(invalidCondition, "Invalid state detected") // Throws ArgumentException if condition is true
    .Build();
```

#### In Blazor Components

```razor
@using Blazor.ClassBuilder

<button @attributes="Attributes">
    Click me
</button>

@code {
    [Parameter] public bool IsDisabled { get; set; }
    [Parameter] public string? Tooltip { get; set; }
    [Parameter] public string? AriaLabel { get; set; }

    private Dictionary<string, object> Attributes => new AttributeBuilder()
        .Add("type", "button")
        .Add("class", "btn btn-primary")
        .AddIf(IsDisabled, "disabled")
        .AddIfFilled("title", Tooltip)
        .AddIfFilled("aria-label", AriaLabel)
        .Build();
}
```

## License

This repository is licensed with the [MIT](LICENSE) license.