# TedToolkit.RoslynHelper

A fluent API for generating C# source code, designed for Roslyn source generators, analyzers, and other code generation workflows.

## Good Fit

This library is useful when you want to compose:

- files, namespaces, and type declarations
- methods, properties, fields, events, indexers, and constructors
- expressions and statements
- XML documentation comments
- conditional compilation blocks
- code-generation syntax derived from Roslyn symbols

It targets `netstandard2.0` and can be distributed as part of analyzer-oriented packages.

## Installation

```xml
<ItemGroup>
  <PackageReference Include="TedToolkit.RoslynHelper" Version="1.0.0" />
</ItemGroup>
```

## Quick Start

```csharp
using TedToolkit.RoslynHelper;
using TedToolkit.RoslynHelper.Syntaxes;

using static TedToolkit.RoslynHelper.SourceComposer;

var code = File()
    .AddUsing(Using("System"))
    .AddNameSpace(NameSpace("Demo.Space")
        .AddMember(Class("Sample").Public
            .AddMember(
                new Method("Run")
                    .Public
                    .Static
                    .AddStatement(1.ToLiteral().Return))))
    .ToCode();
```

The generated output includes the auto-generated file header, `#pragma warning disable`, plus the using directives, namespaces, and members you add.

## Common Capabilities

### 1. Generate types and members

```csharp
using TedToolkit.RoslynHelper.Syntaxes;

var typeDeclaration = new TypeDeclaration("Sample", TypeDeclarationType.CLASS)
    .Public
    .Partial
    .AddBaseType<IDisposable>()
    .AddMember(new Field(DataType.Int, "count"))
    .AddMember(
        new Property(DataType.String, "Name")
            .AddAccessor(new Accessor(AccessorType.GET)))
    .AddMember(
        new Method("Run")
            .AddStatement("count".ToSimpleName().Return));
```

Primary declaration types include:

- `TypeDeclaration`
- `Method`
- `Property`
- `Field`
- `Event`
- `Indexer`
- `Constructor`
- `Operator`
- `Conversion`
- `Enum`
- `Delegate`

### 2. Generate expressions and statements

```csharp
using TedToolkit.RoslynHelper.Syntaxes;

var statement = new IfStatement("ready".ToSimpleName())
    .AddStatement("work".ToSimpleName())
    .Else()
    .AddStatement("fallback".ToSimpleName());

var expression = "items".ToSimpleName()
    .Sub("Count")
    .Add(1.ToLiteral());
```

Common statement types covered by the current library include:

- `Statement`
- `ReturnStatement`
- `IfStatement`
- `ForEachStatement`
- `UsingStatement`
- `TryStatement`
- `SwitchStatement`

### 3. Generate conditional compilation

```csharp
using TedToolkit.RoslynHelper.Syntaxes;
using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

var field = new Field(DataType.Int, "count")
    .AddCondition(PreprocessorExpression.Debug);

var block = new ConditionalCompilationStatement(PreprocessorExpression.Debug)
    .AddStatement("work".ToSimpleName())
    .Else()
    .AddStatement("fallback".ToSimpleName());
```

This is useful when the generated output needs `#if DEBUG`-style structure.

### 4. Convert Roslyn symbols into generation syntax

```csharp
using Microsoft.CodeAnalysis;
using TedToolkit.RoslynHelper;

Parameter parameter = SourceComposer.Parameter(parameterSymbol, compilation);
var dataType = DataType.FromSymbol(typeSymbol, compilation);
var attribute = SourceComposer.Attribute(attributeData, compilation);
var typeParameter = SourceComposer.TypeParameter(typeParameterSymbol, compilation);
```

This is the main bridge between Roslyn analysis data and generated code composition.

### 5. Stamp generated members with generator metadata

```csharp
using static TedToolkit.RoslynHelper.SourceComposer<MyGenerator>;

var method = Method("Run")
    .Public
    .AddStatement("value".ToSimpleName().Return);
```

When you create members through `SourceComposer<TGenerator>`, the library automatically adds `GeneratedCodeAttribute`.

## Notes

- This is a code generation helper library, not a full semantic rewriter
- The main public namespaces used by examples are `TedToolkit.RoslynHelper` and `TedToolkit.RoslynHelper.Syntaxes`
- It is best suited to composing code first, then emitting it through `ToCode()` or `SourceFile.Generate(...)`

## License

LGPL-3.0-or-later.
