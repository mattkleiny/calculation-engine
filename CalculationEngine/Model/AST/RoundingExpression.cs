using System;
using CalculationEngine.Model.Evaluation;

namespace CalculationEngine.Model.AST
{
  public sealed class RoundingExpression : CalculationExpression
  {
    public MidpointRounding      Method { get; }
    public CalculationExpression Value  { get; }

    public RoundingExpression(MidpointRounding method, CalculationExpression value, string label = null)
    {
      Method = method;
      Value  = value;
      Label  = label ?? string.Empty;
    }

    public override decimal Evaluate(CalculationContext context)
    {
      var value = Value.Evaluate(context);

      return Math.Round(value, Method);
    }

    public override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"(Round {Method} {Value})";
    }
  }
}