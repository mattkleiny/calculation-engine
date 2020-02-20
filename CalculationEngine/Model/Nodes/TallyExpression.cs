using System.Linq.Expressions;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class TallyExpression : CalculationExpression
  {
    public EarningsCategory Category { get; }

    public TallyExpression(EarningsCategory category)
    {
      Category = category;
    }

    internal override decimal Evaluate(EvaluationContext context)
    {
      return context.Earnings.SumYearToDates(Category);
    }

    internal override void Explain(ExplanationContext context)
    {
      context.AddStep($"Tally {Category} YTD", this);
    }

    internal override Expression Compile()
    {
      // TODO: use a functional environment pattern to pass the EvaluationContext down here in the expression tree.

      return Expression.Constant(100m);
    }

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"(YTD {Category})";
    }
  }
}