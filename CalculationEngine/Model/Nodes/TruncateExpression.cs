using System;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using CalculationEngine.Model.Visitors;

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

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"(Truncate {Value})";
    }
  }
}