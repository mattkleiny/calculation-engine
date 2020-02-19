using System.Globalization;
using CalculationEngine.Model.Evaluation;

namespace CalculationEngine.Model.AST
{
  public sealed class ConstantExpression : CalculationExpression
  {
    public decimal Value { get; }

    public ConstantExpression(decimal value, string label = null)
    {
      Value = value;
      Label = label ?? string.Empty;
    }

    public override decimal Evaluate(CalculationContext context)
    {
      return Value;
    }

    public override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return Value.ToString("F", CultureInfo.InvariantCulture);
    }
  }
}