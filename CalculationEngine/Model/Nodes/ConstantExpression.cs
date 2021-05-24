using System.Globalization;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using CalculationEngine.Model.Visitors;

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

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return Value.ToString("F", CultureInfo.InvariantCulture);
    }
  }
}