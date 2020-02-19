using System.Globalization;

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

    public override decimal Evaluate() => Value;

    public override T      Accept<T>(CalculationVisitor<T> visitor) => visitor.Visit(this);
    public override string ToString()                               => Value.ToString("F", CultureInfo.InvariantCulture);
  }
}