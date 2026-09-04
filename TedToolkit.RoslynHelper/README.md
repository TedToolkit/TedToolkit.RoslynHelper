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

By default, the generated output includes the auto-generated file header and `#pragma warning disable`, plus the using directives, namespaces, and members you add. File options can preserve warnings and enable nullable checking.

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

### 6. Compose async code

```csharp
var call = "ReadAsync".ToSimpleName().Invoke("cancellationToken".ToSimpleName());
var method = new Method("ExecuteAsync", new ReturnType(DataType.TaskOf(DataType.Int)))
    .Public.Async
    .AddParameter(new Parameter(DataType.FromType<CancellationToken>(), "cancellationToken"))
    .AddStatement(call.ConfigureAwait(false).Await().Return);
```

`.Async` sets `Method.IsAsync` and preserves the declared return type. The same `Method` can be added as a local function through `AddStatement`. `DataType.Task`, `TaskOf(resultType)`, `ValueTask`, and `ValueTaskOf(resultType)` create fresh mutable type representations; the result type can come from `DataType.FromSymbol`.

`.Await()` emits only `await`. Add `.ConfigureAwait(false)` explicitly when appropriate; the expression overload accepts a variable or other configuration expression. Use parentheses explicitly when composing compound operands or accessing the awaited result:

```csharp
task.Coalesce(fallback).Parenthesized.ConfigureAwait(false).Await();
call.Await().Parenthesized.Sub("Length"); // (await ReadAsync(...)).Length
```

Async iteration and disposal use the existing statement builders:

```csharp
var iteration = "items".ToSimpleName().ForEach(DataType.Var, "item").Await
    .AddStatement("Process".ToSimpleName().Invoke("item".ToSimpleName()));
var lifetime = "resource".ToSimpleName().Using.Await.AddStatement(iteration);
```

These generate `await foreach (...)` and `await using (...) { ... }`; set `IsAwait = false` to return to synchronous syntax. The target compilation must supply the relevant awaitable types and language support.

### 7. Pass arguments and configure generated files

`Invoke(params IExpression[])`, `AddArgument(IExpression)`, and `AddArguments(params IExpression[])` accept positional expressions. Existing `Argument` objects remain available for named and `ref`/`in`/`out` arguments:

```csharp
var call = "Update".ToSimpleName().Invoke(1.ToLiteral())
    .AddArgument(new Argument("value".ToSimpleName()).Ref);
var creation = new DataType("global::System.Version").New.AddArguments(1.ToLiteral(), 2.ToLiteral());

var file = new SourceFile
{
    DisableWarnings = false,
    NullableContext = Microsoft.CodeAnalysis.NullableContextOptions.Enable,
}.AddMember(new TypeDeclaration("GlobalWorker", TypeDeclarationType.CLASS)
    .AddMember(new Method("Run").Public));
```

`SourceFile.Members` and `AddMember` emit declarations in the global namespace. File-level using directives precede assembly attributes; global members precede named namespaces. Empty namespaces use blocks when sharing a file with global members or other namespaces.

`DisableWarnings` defaults to `true`. `NullableContext` defaults to `null` (no directive), and supports `Disable`, `Enable`, `Annotations`, and `Warnings`. To report nullable warnings, enable their nullable context and set `DisableWarnings = false`.

Empty `TryStatement`, `CatchClause`, and `FinallyClause` bodies always emit `{}`. A `TryStatement` still needs a catch or finally clause to form valid C#.

## Notes

- This is a code generation helper library, not a full semantic rewriter
- String and character literals escape C# special characters. Floating-point literals support NaN and infinity; doubles include a `D` suffix to preserve their type and negative zero.
- The main public namespaces used by examples are `TedToolkit.RoslynHelper` and `TedToolkit.RoslynHelper.Syntaxes`
- It is best suited to composing code first, then emitting it through `ToCode()` or `SourceFile.Generate(...)`

## License

LGPL-3.0-or-later.