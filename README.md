# TedToolkit.RoslynHelper

A fluent API library for programmatically generating C# source code, designed for use in Roslyn incremental source generators and analyzers.

[![NuGet](https://img.shields.io/nuget/v/TedToolkit.RoslynHelper)](https://www.nuget.org/packages/TedToolkit.RoslynHelper)
[![License: LGPL-3.0-or-later](https://img.shields.io/badge/license-LGPL--3.0--or--later-blue)](COPYING.LESSER)

## Features

- **Fluent Code Generation** - Chainable API to build classes, structs, records, interfaces, enums, delegates, and more
- **Full C# Member Support** - Methods, properties, fields, events, constructors, indexers, operators, and conversions
- **Expression & Statement Builders** - Compose expressions (literals, binary ops, casts, invocations, object creation) and statements (if, foreach, switch, try-catch, using, return)
- **XML Documentation** - Generate `///` doc comments (summary, param, returns, remarks, exception, example, etc.)
- **Type System** - Handle generics, nullable types, pointers, ref/scoped parameters, and nested types
- **High Performance** - Built on [ZString](https://github.com/Cysharp/ZString) for allocation-efficient string building
- **Roslyn Integration** - Targets `netstandard2.0`, distributable as an analyzer component in NuGet packages
- **Automatic `GeneratedCodeAttribute`** - Tracks which generator produced the code

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

// Build a source file with a class
var code = File()
    .AddNameSpace(NameSpace("MyApp.Models")
        .AddMember(Class("Person").Public.Partial
            .AddMember(Property<string>("Name").Public
                .AddAccessor(Accessor(AccessorType.GET))
                .AddAccessor(Accessor(AccessorType.SET)))
            .AddMember(Property<int>("Age").Public
                .AddAccessor(Accessor(AccessorType.GET))
                .AddAccessor(Accessor(AccessorType.SET)))
            .AddMember(Method("Greet", ReturnType(DataType.String)).Public
                .AddStatement("$\"Hello, I'm {Name}\"".ToSimpleName().Return))))
    .ToCode();
```

## Usage Examples

### Type Declarations

```csharp
// Class with modifiers
Class("MyClass").Public.Static.Unsafe.Partial

// Struct, record, interface
Struct("MyStruct").Public
Record("MyRecord").Public
Interface("IMyInterface").Public

// With base types and generics
Class("MyList").Public
    .AddBaseType<IDisposable>()
    .AddTypeParameter(TypeParameter("T").In
        .AddNewConstraint()
        .AddConstraint<IComparable>())
```

### Members

```csharp
// Method
Method("Calculate", ReturnType(DataType.Int)).Public
    .AddParameter(Parameter<int>("x"))
    .AddParameter(Parameter<int>("y"))
    .AddStatement("x + y".ToSimpleName().Return)

// Property with default value
Property<long>("Count").Internal
    .AddAccessor(Accessor(AccessorType.GET))
    .AddDefault(10.ToLiteral())

// Field
Field<long>("_count").Private.Readonly

// Event
Event<Action<int>>("ItemChanged").Public

// Constructor with initializer
Constructor().Public
    .AddInitializer(new ConstructorInitializer(false))

// Delegate
Delegate("MyCallback").Public

// Enum with members
Enum("Status").Public
    .AddEnumMember(EnumMember("Active"))
    .AddEnumMember(EnumMember("Inactive"))
```

### Expressions

```csharp
// Literals
10.ToLiteral()
"hello".ToLiteral()

// Simple name
"myVariable".ToSimpleName()

// Object creation
new ObjectCreationExpression(DataType.FromType<int>())
    .AddArgument(Argument(10.ToLiteral()))

// Collections
new CollectionExpression()
    .AddElement(new ObjectCreationExpression(DataType.FromType<int>()))
```

### Statements

```csharp
// ForEach
new ForEachStatement(DataType.Var, "item", new SimpleNameExpression("source"))

// Variable declaration
new VariableExpression(DataType.Int, "count")
    .AddDefault(10.ToLiteral())

// Switch
new SwitchStatement("value".ToSimpleName())
    .AddSection(new SwitchSection()
        .AddLabel(new SwitchLabel(1.ToLiteral()))
        .AddStatement("break".ToSimpleName()))
```

### XML Documentation

```csharp
Class("MyClass").Public
    .AddRootDescription(new DescriptionSummary(
        new DescriptionText("This is my class.")))
    .AddMember(Method("DoWork").Public
        .AddParameter(Parameter<int>("count")
            .AddDescription(new DescriptionText("Number of iterations."))))
```

### Parameters

```csharp
// Typed parameter with default
Parameter<int>("item").AddDefault(10.ToLiteral())

// Scoped in parameter
Parameter(DataType.Int.ScopedIn, "item").This

// Nullable parameter
Parameter(DataType.Int.Null.ScopedIn, "item")

// From Roslyn symbols
Parameter(parameterSymbol, compilation)
```

### Attributes

```csharp
Method("Method")
    .AddAttribute(Attribute<MethodImplAttribute>()
        .AddArgument(Argument(MethodImplOptions.AggressiveInlining.ToExpression())))
```

## Project Structure

```
TedToolkit.RoslynHelper/           Main library (netstandard2.0)
  Extensions/                      Roslyn symbol/syntax helper extensions
  Generators/
    Syntaxes/                      Code generation syntax nodes
      Members/                     Type declarations, methods, properties, etc.
      Expressions/                 Expression builders
      Statements/                  Statement builders (if, foreach, switch, etc.)
      Descriptions/                XML documentation comment builders
    Interfaces/                    Fluent API interfaces
    Enums/                         Modifier and type enums
  Names/                           Type naming system
TedToolkit.RoslynHelper.Tests/     Unit tests (TUnit)
TedToolkit.RoslynHelper.Benchmarks/ Performance benchmarks (BenchmarkDotNet)
Build/                             Build orchestration
```

## Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| [Microsoft.CodeAnalysis.CSharp](https://www.nuget.org/packages/Microsoft.CodeAnalysis.CSharp) | 5.0.0 | Roslyn compiler APIs |
| [ZString](https://github.com/Cysharp/ZString) | 2.6.0 | High-performance string building |
| [System.Memory](https://www.nuget.org/packages/System.Memory) | 4.6.3 | `Span<T>` / `ReadOnlySpan<T>` support |

## License

This project is licensed under the [GNU Lesser General Public License v3.0 or later](COPYING.LESSER).
See [COPYING](COPYING) and [COPYING.LESSER](COPYING.LESSER) for full license text.
