# Calculation Engine

A simple graph-based calculation engine with a fluent builder interface that can compile down into a C# delegate.

This is an attempt to swap from an imperative calculation engine to a declarative one, with support for evaluation, compilation and explanation at runtime.

## How it works:

This engine permits you to declaratively construct calculations prior to evaluation, with labels and variables as appropriate,
an example might be:

``` c#
public static CompiledCalculation Calculation { get; } = CompiledCalculation.Create(() =>
{
  var earnings   = YTD(Earnings);
  var allowances = YTD(Allowances);
  var deductions = YTD(Deductions);
  var leave      = YTD(Leave);

  var totalEarnings = Variable("A", Sum(earnings, allowances, deductions, leave));

  var b = Variable("B", Round(totalEarnings - allowances));
  var c = Variable("C", Round(totalEarnings - deductions));
  var d = Variable("D", Round(totalEarnings - leave));

  var e = Variable("E", Truncate(Tax(PAYG, totalEarnings)));

  return b + c + d - e;
});
```

Once a calculation has been defined, it can be executed at any time like this:

``` c#
Calculation.Execute(new EvaluationContext())
```

Where the `EvaluationContext` allows access to the database, metadata, tax tables, etc.

Similarly, the calculation can be explained in a structured way like this:

``` c#
var explanation = Calculation.Explain(new EvaluationContext())

foreach (var step in explanation) 
{
  Console.WriteLine(step.Description);
}
```

## Things to clean-up:

* Add some more node types, explore different types of calculations and permit different result types per tree

## Benchmarks

The calculation can either be interpreted directly from an internal AST representation, or compiled down into a 
C# delegate and executed directly on the runtime.

Here are some benchmarks comparing the two approaches:

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
