using CalculationEngine.Model.Evaluation;

namespace CalculationEngine.Model.AST
{
  public sealed class ApplyTaxExpression : CalculationExpression
  {
    public TaxCategory           Category { get; }
    public CalculationExpression Value    { get; }

    public ApplyTaxExpression(TaxCategory category, CalculationExpression value, string label = null)
    {
      Category = category;
      Value    = value;
      Label    = label ?? string.Empty;
    }

    public override decimal Evaluate(CalculationContext context)
    {
      var table  = context.TaxTables[Category];
      var amount = Value.Evaluate(context);

      return amount * table[amount];
    }

    public override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"({Category} Tax {Value})";
    }
  }
}