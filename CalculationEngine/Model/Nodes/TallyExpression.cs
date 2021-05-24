using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using CalculationEngine.Model.Utilities;
using CalculationEngine.Model.Visitors;

namespace CalculationEngine.Model.Nodes
{
  internal sealed record TallyExpression(EarningsCategory Categories, AnnualisationMethod Method) : CalculationExpression
  {
    internal override decimal Evaluate(EvaluationContext context)
    {
      var key = $"Earnings:({Categories.ToPermutationString()})";

      return context.Results.GetOrCompute(key, () => context.Earnings.CalculateEarnings(Categories, Method));
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
      return $"(YTD {Categories.ToPermutationString()} as {Method})";
    }
  }
}