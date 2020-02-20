using System;
using System.Linq.Expressions;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class TruncateExpression : CalculationExpression
  {
    public CalculationExpression Value { get; }

    public TruncateExpression(CalculationExpression value)
    {
      Value = value;
    }

    internal override decimal Evaluate(EvaluationContext context)
    {
      return Math.Truncate(Value.Evaluate(context));
    }

    internal override void Explain(ExplanationContext context)
    {
      Value.Explain(context);
    }

    internal override Expression Compile()
    {
      var method = typeof(Math).GetMethod(nameof(Math.Truncate), new[] { typeof(decimal) });
      if (method == null)
      {
        throw new Exception($"Unable to locate {nameof(Math)}.{nameof(Math.Truncate)} method; has the version of the .NET framework changed?");
      }

      var argument1 = Value.Compile();

      return Expression.Call(method, argument1);
    }

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"(Truncate {Value})";
    }
  }
}