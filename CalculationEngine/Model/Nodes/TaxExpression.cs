using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class TaxExpression : ClosedExpression1
  {
    public TaxCategory           Category { get; }
    public CalculationExpression Value    { get; }

    public TaxExpression(TaxCategory category, CalculationExpression value)
    {
      Category = category;
      Value    = value;
    }

    protected override CalculationExpression Parameter1 => Value;

    protected override decimal Execute(EvaluationContext context, decimal cumulative)
    {
      var table = context.TaxTables[Category];
      var (a, b) = table[cumulative];

      return a * cumulative - b;
    }

    internal override void Explain(ExplanationContext context)
    {
      Value.Explain(context);
    }

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"(Calculate {Category} Tax {Value})";
    }
  }
}