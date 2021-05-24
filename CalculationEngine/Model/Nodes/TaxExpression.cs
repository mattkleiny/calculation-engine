using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using CalculationEngine.Model.Visitors;

namespace CalculationEngine.Model.Nodes
{
  internal sealed record TaxExpression(TaxCategory Category, CalculationExpression Value) : CalculationExpression
  {
    internal override decimal Evaluate(EvaluationContext context)
    {
      var cumulative = Value.Evaluate(context);
      var table      = context.TaxTables[Category];
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