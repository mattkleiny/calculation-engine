# Calculation Engine

A simple graph-based calculation engine with a simple fluent syntax and extensibility for new operators.

This is an attempt to help move from an imperative calculation engine to a more flexible and declarative one,
with support for evaluation and explanation at runtime.

## How it works:

*N.B: All the calculations below are bogus, and are simply meant to illustrate the syntax/structure of the DSL*.

This engine permits you to declaratively construct `Calculation`s prior to evaluation, with `Label`s, `Variables` and
various different mathematical operations, as appropriate.

All the calculations are expected to produce a `decimal` out the other end of the pipeline, and execution of the calculations
are deferred until explicitly executed.

For example:

``` c#
public static Calculation ExampleCalculation { get; } = Calculation.Create(() =>
{
  var ordinaryEarnings = YTD(OrdinaryEarnings);
  var allowances       = YTD(Allowances);
  var deductions       = YTD(Deductions);
  var leave            = YTD(Leave);

  var a = Sum(ordinaryEarnings, allowances, deductions, leave);

  var b = Variable("B", Round(allowances));
  var c = Variable("C", Round(deductions));
  var d = Variable("D", Round(leave));

  var e = Variable("E", Truncate(Tax(PAYG, a)));

  return a - (b + c + d) - e / 2m;
});
```

Once a calculation has been defined, it can be executed at any time like this:

``` c#
Calculation.Evaluate(new EvaluationContext())
```

Where the `EvaluationContext` allows access to the database, metadata, tax tables, any other external data that is not 
explicitly baked into the calculation.

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

## Compilation

Earlier versions of this implementation also support compilation from the AST model down into a C# delegate. In almost
all benchmarks this provided no benefit and frequently resulted in decreased performance, especially starting from .NET 5.

It's believed that because the runtime/online LINQ-style delegate compilation system in C# lacks the optimization of the
standard C# compiler and that the standard compiler is able to optimize the AST instructions sufficiently that a more complex
solution is simply unwarranted.