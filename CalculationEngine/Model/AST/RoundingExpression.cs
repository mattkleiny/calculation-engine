using System;

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

    public override decimal Evaluate() => Math.Round(Value.Evaluate(), Method);

    public override T      Accept<T>(CalculationVisitor<T> visitor) => visitor.Visit(this);
    public override string ToString()                               => $"Round {Value} {Method}";
  }
}