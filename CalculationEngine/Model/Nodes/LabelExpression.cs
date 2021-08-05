using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed record LabelExpression(string Label, CalculationExpression Expression) : CalculationExpression
  {
    internal override decimal Evaluate(EvaluationContext context)
    {
      return Expression.Evaluate(context);
    }

    internal override void Explain(ExplanationContext context)
    {
      Expression.Explain(context);

      context.AddStep(Label, this);
    }

    public override string ToString()
    {
      return Expression.ToString();
    }
  }
}