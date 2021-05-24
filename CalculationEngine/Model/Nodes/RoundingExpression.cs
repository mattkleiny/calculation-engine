using System;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using CalculationEngine.Model.Visitors;

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

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"(Round {Rounding} {Value})";
    }
  }
}