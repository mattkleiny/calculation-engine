using System;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed record RoundingExpression(CalculationExpression Value, MidpointRounding Rounding) : CalculationExpression
  {
    internal override decimal Evaluate(EvaluationContext context)
    {
      var value = Value.Evaluate(context);

      return Math.Round(value, Rounding);
    }

    internal override void Explain(ExplanationContext context)
    {
      Value.Explain(context);
    }

    public override string ToString()
    {
      return $"(Round {Rounding} {Value})";
    }
  }
}