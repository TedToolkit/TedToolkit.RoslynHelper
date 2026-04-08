# TedToolkit.RoslynHelper

A fluent API library for programmatically generating C# source code, designed for use in Roslyn incremental source generators and analyzers.

## Features

- Fluent, chainable API to build classes, structs, records, interfaces, enums, delegates, and more
- Full C# member support: methods, properties, fields, events, constructors, indexers, operators, conversions
- Expression and statement builders (if, foreach, switch, try-catch, using, return)
- XML documentation comment generation (summary, param, returns, remarks, etc.)
- Generics, nullable types, pointers, ref/scoped parameters, nested types
- High-performance string building via [ZString](https://github.com/Cysharp/ZString)
- Targets `netstandard2.0` - distributable as an analyzer component in NuGet packages
- Automatic `GeneratedCodeAttribute` tracking

## Installation

```xml
<ItemGroup>
    <PackageReference Include="TedToolkit.RoslynHelper" Version="1.0.0" />
</ItemGroup>
```

## Quick Start

```csharp
using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

using static SourceComposer;
using static SourceComposer<MyGenerator>;

var code = File()
    .AddNameSpace(NameSpace("MyApp.Models")
        .AddMember(Class("Person").Public.Partial
            .AddMember(Property<string>("Name").Public
                .AddAccessor(Accessor(AccessorType.GET))
                .AddAccessor(Accessor(AccessorType.SET)))
            .AddMember(Method("Greet", ReturnType(DataType.String)).Public
                .AddStatement("$\"Hello, I'm {Name}\"".ToSimpleName().Return))))
    .ToCode();
```

### Type Declarations

```csharp
Class("MyClass").Public.Static.Unsafe.Partial
Struct("MyStruct").Public.Readonly
Record("MyRecord").Public
Interface("IMyInterface").Public

// With base types and generic constraints
Class("MyList").Public
    .AddBaseType<IDisposable>()
    .AddTypeParameter(TypeParameter("T").In
        .AddNewConstraint()
        .AddConstraint<IComparable>())
```

### Members

```csharp
// Method with parameters
Method("Calculate", ReturnType(DataType.Int)).Public
    .AddParameter(Parameter<int>("x"))
    .AddParameter(Parameter<int>("y"))

// Property with default value
Property<long>("Count").Internal
    .AddAccessor(Accessor(AccessorType.GET))
    .AddDefault(10.ToLiteral())

// Field, event, delegate
Field<long>("_count").Private.Readonly
Event<Action<int>>("ItemChanged").Public
Delegate("MyCallback").Public

// Enum
Enum("Status").Public
    .AddEnumMember(EnumMember("Active"))
    .AddEnumMember(EnumMember("Inactive"))
```

### XML Documentation

```csharp
Class("MyClass").Public
    .AddRootDescription(new DescriptionSummary(
        new DescriptionText("This is my class.")))
```

## Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| [Microsoft.CodeAnalysis.CSharp](https://www.nuget.org/packages/Microsoft.CodeAnalysis.CSharp) | 5.0.0 | Roslyn compiler APIs |
| [ZString](https://github.com/Cysharp/ZString) | 2.6.0 | High-performance string building |
| [System.Memory](https://www.nuget.org/packages/System.Memory) | 4.6.3 | Span/ReadOnlySpan support |

## License

[LGPL-3.0-or-later](https://github.com/TedToolkit/TedToolkit.RoslynHelper/blob/main/COPYING.LESSER)
