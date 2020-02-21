using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using CalculationEngine.Model.Utilities;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class TallyExpression : ClosedExpression0
  {
    public EarningsCategory Categories { get; }

    public TallyExpression(EarningsCategory categories)
    {
      Categories = categories;
    }

    protected override decimal Execute(EvaluationContext context)
    {
      var key = $"Earnings:({Categories.ToPermutationString()})";

      return context.Results.GetOrCompute(key, () => context.Earnings.SumYearToDate(Categories));
    }

    internal override void Explain(ExplanationContext context)
    {
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