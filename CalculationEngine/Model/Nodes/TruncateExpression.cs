using System;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed record TruncateExpression(CalculationExpression Value) : CalculationExpression
  {
    internal override decimal Evaluate(EvaluationContext context)
    {
      return Math.Truncate(Value.Evaluate(context));
    }

    internal override void Explain(ExplanationContext context)
    {
      Value.Explain(context);
    }

    public override string ToString()
    {
      return $"(Truncate {Value})";
    }
  }
}