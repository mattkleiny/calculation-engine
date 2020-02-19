using System;
using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.AST
{
  public sealed class RoundingExpression : CalculationExpression
  {
    public MidpointRounding      Method { get; }
    public CalculationExpression Value  { get; }
    public string                Label  { get; }

    public RoundingExpression(MidpointRounding method, CalculationExpression value, string label = null)
    {
      Method = method;
      Value  = value;
      Label  = label ?? string.Empty;
    }

    public override decimal Evaluate(EvaluationContext context)
    {
      var value = Value.Evaluate(context);

      return Math.Round(value, Method);
    }

    public override Expression Compile(CompilationContext context)
    {
      var method = typeof(Math).GetMethod(nameof(Math.Round), new[] { typeof(decimal), typeof(MidpointRounding) });

      if (method == null)
      {
        throw new Exception($"Unable to locate {nameof(Math)}.{nameof(RoundingExpression)} method; has the version of the .NET framework changed?");
      }

      var argument1 = Value.Compile(context);
      var argument2 = Expression.Constant(Method);

      return Expression.Call(method, argument1, argument2);
    }

    public override void Explain(ExplanationContext context)
    {
      if (!string.IsNullOrEmpty(Label))
      {
        context.Steps.Add(new CalculationStep(Label, ToString(), Evaluate(context.Evaluation)));
      }
    }

    public override string ToString()
    {
      return $"(Round {Method} {Value})";
    }
  }
}