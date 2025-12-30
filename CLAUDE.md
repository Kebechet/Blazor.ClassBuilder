# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands

```bash
# Solution is in src folder
dotnet restore src/Blazor.ClassBuilder.sln
dotnet build src/Blazor.ClassBuilder.sln
dotnet test src/Blazor.ClassBuilder.sln

# Run single test
dotnet test src/Blazor.ClassBuilder.sln --filter "FullyQualifiedName~TestMethodName"

# Build with release configuration (also generates NuGet package)
dotnet build src/Blazor.ClassBuilder.sln --configuration Release
```

## Architecture

This is a .NET Standard 2.1 library providing fluent builders for Blazor components:

```
src/Blazor.ClassBuilder/
├── ClassBuilder.cs          # CSS class string builder ("btn active")
├── StyleBuilder.cs          # Inline CSS style builder ("color: red;")
├── AttributeBuilder.cs      # HTML attribute dictionary builder
└── Extensions/
    ├── DoubleExtensions.cs  # ToCssString() for locale-safe decimals
    └── AttributeBuilderExtensions.cs  # Internal IsNullOrEmpty check

tests/Blazor.ClassBuilder.Tests/
└── *Tests.cs                # xUnit tests with Arrange/Act/Assert pattern
```

## Key Design Patterns

- **Fluent API**: All builders return `this` for method chaining
- **Locale-safe decimals**: `StyleBuilder` uses `InvariantCulture` to ensure decimal points (`.`) instead of locale-specific commas (`,`) for valid CSS
- **Conditional methods**: `AddIf()` and `AddIfElse()` handle conditional logic without breaking the fluent chain

## Versioning

Version is managed in `src/Blazor.ClassBuilder/Blazor.ClassBuilder.csproj`:
```xml
<Version>1.2.0</Version>
<PackageReleaseNotes>Description of changes</PackageReleaseNotes>
```

CI auto-publishes to NuGet when version changes (creates git tag on publish).
