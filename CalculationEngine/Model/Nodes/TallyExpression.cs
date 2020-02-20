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
      // TODO: sample this from somewhere on the context
      return 100m;
    }

    internal override void Explain(ExplanationContext context)
    {
      context.AddStep($"Tally {Category} YTD", this);
    }

    internal override Expression Compile()
    {
      // TODO: sample this from somewhere on the context
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