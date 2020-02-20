using System.Linq.Expressions;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class TaxExpression : CalculationExpression
  {
    public TaxCategory           Category { get; }
    public CalculationExpression Value    { get; }
    public string                Label    { get; }

    public TaxExpression(TaxCategory category, CalculationExpression value, string label = null)
    {
      Category = category;
      Value    = value;
      Label    = label ?? string.Empty;
    }

    internal override decimal Evaluate(EvaluationContext context)
    {
      var amount = Value.Evaluate(context);

      var table = context.TaxTables[Category];
      var (a, b) = table[amount];

      return a * amount - b;
    }

    internal override void Explain(ExplanationContext context)
    {
      Value.Explain(context);

      if (!string.IsNullOrEmpty(Label))
      {
        context.AddStep(Label, this);
      }
    }

    internal override Expression Compile()
    {
      // TODO: use a functional environment pattern to pass the EvaluationContext down here in the expression tree.

      var left  = Value.Compile();
      var right = Expression.Constant(0.20m);

      return Expression.MultiplyChecked(left, right);
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