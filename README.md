# Calculation Engine

A simple graph-based calculation model with a fluent builder interface that can compile into a C# delegate.

This is an attempt to swap from an imperative calculation engine to a declarative one, with support for evaluation, compilation and explanation.

## Things to clean-up:

* Add some more node types, explore different types of calculations and permit different result types per tree

## Benchmarks

``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-7820HQ CPU 2.90GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.100
  [Host]     : .NET Core 3.1.0 (CoreCLR 4.700.19.56402, CoreFX 4.700.19.56404), X64 RyuJIT
  Job-AJITFZ : .NET Core 3.1.0 (CoreCLR 4.700.19.56402, CoreFX 4.700.19.56404), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|               Method |     Mean |    Error |   StdDev |   Median |
|--------------------- |---------:|---------:|---------:|---------:|
|   ExecuteCalculation | 401.0 ns |  8.80 ns | 25.66 ns | 399.3 ns |
| InterpretCalculation | 747.3 ns | 14.58 ns | 36.03 ns | 735.0 ns |
