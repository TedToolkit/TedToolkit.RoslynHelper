using BenchmarkDotNet.Running;

using TedToolkit.RoslynHelper.Benchmarks;

Console.WriteLine("Hello");

var instance = new RoslynRunner();
Console.WriteLine(instance.Roslyn());
Console.WriteLine(instance.Helper());

BenchmarkRunner.Run<RoslynRunner>();