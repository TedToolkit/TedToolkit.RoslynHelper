# TedToolkit.RoslynHelper

Developer README for maintaining this repository.

If you only want to consume the package, read [TedToolkit.RoslynHelper/README.md](TedToolkit.RoslynHelper/README.md). That file is also the NuGet package README.

## Repository Scope

This repository contains a Roslyn-oriented C# source generation helper library targeting `netstandard2.0`. It is intended to be referenced by source generators, analyzers, or other code generation pipelines.

Current projects in the repository:

- `TedToolkit.RoslynHelper`
  Main library.
- `TedToolkit.RoslynHelper.Tests`
  Test project covering member generation, expressions, statements, conditional compilation, Roslyn symbol conversion, and end-to-end composition.
- `TedToolkit.RoslynHelper.Benchmarks`
  Benchmark project.
- `Build`
  Build orchestration project.
- `externals/TedToolkit`
  Shared external props and build configuration.

## README Split

- Root [README.md](README.md)
  Developer-facing documentation for this repository.
- Project [TedToolkit.RoslynHelper/README.md](TedToolkit.RoslynHelper/README.md)
  Consumer-facing documentation and NuGet package README.

These two files should not duplicate each other.

## What The Library Actually Supports

Based on the current code and tests, the library presently supports:

- Generating `class`, `struct`, `record`, `record struct`, `interface`, `enum`, and `delegate`
- Generating `field`, `property`, `event`, `indexer`, `constructor`, `method`, `operator`, and `conversion`
- Generating expressions and statements
  Including `return`, `if/else`, `foreach`, `switch`, `try/catch/finally`, and `using`
- Generating XML documentation comments
- Generating conditional compilation structures
  Including file-level, member-level, and statement-level `#if / #elif / #else / #endif`
- Converting Roslyn symbols
  Including `ITypeSymbol`, `IParameterSymbol`, `AttributeData`, and type parameter constraints
- Automatically adding `GeneratedCodeAttribute` through `SourceComposer<TGenerator>`

## Common Commands

```powershell
dotnet build TedToolkit.RoslynHelper.slnx -c Release
dotnet test TedToolkit.RoslynHelper.Tests/TedToolkit.RoslynHelper.Tests.csproj -c Release
dotnet run --project TedToolkit.RoslynHelper.Benchmarks/TedToolkit.RoslynHelper.Benchmarks.csproj -c Release
```

For most maintenance work, the first two commands are enough.

## Packaging Constraints

Current packaging behavior comes from [TedToolkit.RoslynHelper.csproj](TedToolkit.RoslynHelper/TedToolkit.RoslynHelper.csproj) and [NugetPackage.props](externals/TedToolkit/props/NugetPackage.props):

- The package targets `netstandard2.0`
- NuGet package generation is enabled in `Release`
- `TedToolkit.RoslynHelper/README.md` is used as `PackageReadmeFile`
- The main DLL, `ZString.dll`, and `System.Memory.dll` are packed into `analyzers/dotnet/cs`

That means:

- The project README must stay consumer-focused
- The NuGet README examples should stay short and immediately usable
- Public capability claims should be grounded in the current API and tests, not assumptions

## Documentation Rules

When updating documentation for this repository:

- Only describe capabilities that exist now
- Prefer examples that match tested APIs
- Do not describe directories or namespaces that no longer exist
- Do not document speculative future features as current behavior
- Keep the project README focused on installation, usage, and fit
- Keep the root README focused on repository maintenance, verification, and packaging context

## License

Licensed under [LGPL-3.0-or-later](COPYING.LESSER).
