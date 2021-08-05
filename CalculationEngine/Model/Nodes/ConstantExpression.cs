using System.Globalization;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed record ConstantExpression(decimal Value) : CalculationExpression
  {
    internal override decimal Evaluate(EvaluationContext context)
    {
      return Value;
    }

    internal override void Explain(ExplanationContext context)
    {
    }

    public override string ToString()
    {
      return Value.ToString("F", CultureInfo.InvariantCulture);
    }
  }
}