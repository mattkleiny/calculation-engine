using System.Collections.Generic;
using System.Linq;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using CalculationEngine.Model.Visitors;

namespace CalculationEngine.Model.Nodes
{
  internal sealed record SumExpression(IEnumerable<CalculationExpression> Expressions) : CalculationExpression
  {
    internal override decimal Evaluate(EvaluationContext context)
    {
      return Expressions.Sum(expression => expression.Evaluate(context));
    }

    internal override void Explain(ExplanationContext context)
    {
      foreach (var expression in Expressions)
      {
        expression.Explain(context);
      }
    }

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"(Σ {string.Join(" + ", Expressions.Select(_ => _.ToString()))})";
    }
  }
}