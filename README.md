# Calculation Engine

A simple graph-based calculation engine with a fluent syntax that can compiled down into a C# delegate.

This is an attempt to help move from an imperative calculation engine to a more flexible and declarative one,
with support for evaluation, compilation and explanation at runtime.

## How it works:

*N.B: All the calculations below are bogus, and are simply meant to illustrate the syntax/structure of the DSL*.

This engine permits you to declaratively construct `Calculation`s prior to evaluation, with `Label`s, `Variables` and
various different mathematical operations, as appropriate.

All the calculations are expected to produce a `decimal` out the other end of the pipeline, and execution of the calculations
are deferred until explicitly executed.

For example:

``` c#
public static CompiledCalculation Calculation { get; } = CompiledCalculation.Create(() =>
{
  var ordinaryEarnings = YTD(OrdinaryEarnings);
  var allowances       = YTD(Allowances);
  var deductions       = YTD(Deductions);
  var leave            = YTD(Leave);

  var totalEarnings = Variable("A", Sum(ordinaryEarnings, allowances, deductions, leave));

  var b = Variable("B", Round(totalEarnings - allowances));
  var c = Variable("C", Round(totalEarnings - deductions));
  var d = Variable("D", Round(totalEarnings - leave));

  var e = Variable("E", Truncate(Tax(PAYG, totalEarnings)));

  return b + c - d * e / 2m;
});
```

Once a calculation has been defined, it can be executed at any time like this:

``` c#
Calculation.Execute(new EvaluationContext())   // using the compiled delegate
Calculation.Interpret(new EvaluationContext()) // using the in-memory AST
```

Where the `EvaluationContext` allows access to the database, metadata, tax tables, any other external data that is not 
explicitly baked into the calculation.

The `CompiledCalculation`, in this case, is a container for a compiled C# delegate that is ready to run directly on the 
CLR. It's also possible to interpret the resultant AST tree with the `.Interpret(EvaluationContext)` method.

Similarly, the calculation can be explained in a structured way like this:

``` c#
var explanation = Calculation.Explain(new EvaluationContext())

foreach (var step in explanation) 
{
  Console.WriteLine(step.Description);
}
```

This yields a series of `CalculationStep`s which describes important parts of the calculation, and the sum figures available
at various points of the evaluation, and can be marked up with `Label`s, `Variable`s and other similar expressions.

Such an `explanation` could be used in a UI to render calculation details to users, or to help understand the underlying logic
of a particularly complex calculation.

## Remaining Tasks:

* Add some more node types, explore different types of calculations and permit different result types per tree.
* Wire up passing of `EvaluationContext` down into the AST's `Compile()` method with an environment pattern in the resultant LINQ tree.

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
