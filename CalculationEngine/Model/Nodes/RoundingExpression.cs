using System;
using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class RoundingExpression : CalculationExpression
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

    internal override decimal Evaluate(EvaluationContext context)
    {
      var value = Value.Evaluate(context);

      return Math.Round(value, Method);
    }

    internal override Expression Compile(CompilationContext context)
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

    internal override void Explain(ExplanationContext context)
    {
      Value.Explain(context);

      if (!string.IsNullOrEmpty(Label))
      {
        context.Steps.Add(new CalculationStep(Label, ToString(), Evaluate(context.Evaluation)));
      }
    }

    internal override T Accept<T>(IVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"(Round {Method} {Value})";
    }
  }
}