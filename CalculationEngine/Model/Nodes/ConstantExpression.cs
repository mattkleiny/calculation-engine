using System.Globalization;
using System.Linq.Expressions;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class ConstantExpression : CalculationExpression
  {
    public decimal Value { get; }
    public string  Label { get; }

    public ConstantExpression(decimal value, string label = null)
    {
      Value = value;
      Label = label ?? string.Empty;
    }

    internal override decimal Evaluate(EvaluationContext context)
    {
      return Value;
    }

    internal override void Explain(ExplanationContext context)
    {
      if (!string.IsNullOrEmpty(Label))
      {
        context.AddStep(Label, this);
      }
    }

    internal override Expression Compile()
    {
      return Expression.Constant(Value);
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