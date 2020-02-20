using System.Linq.Expressions;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using CalculationEngine.Model.Utilities;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class TallyExpression : CalculationExpression
  {
    public EarningsCategory Categories { get; }

    public TallyExpression(EarningsCategory categories)
    {
      Categories = categories;
    }

    internal override decimal Evaluate(EvaluationContext context)
    {
      return context.Earnings.SumYearToDates(Categories);
    }

    internal override void Explain(ExplanationContext context)
    {
      context.AddStep($"Tally {Categories} YTD", this);
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
      return $"(YTD {Categories.ToPermutationString()})";
    }
  }
}