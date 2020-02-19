using System.Globalization;
using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Tree
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

    public override decimal Evaluate(EvaluationContext context)
    {
      return Value;
    }

    public override Expression Compile(CompilationContext context)
    {
      return Expression.Constant(Value);
    }

    public override void Explain(ExplanationContext context)
    {
      if (!string.IsNullOrEmpty(Label))
      {
        context.Steps.Add(new CalculationStep(Label, ToString(), Evaluate(context.Evaluation)));
      }
    }

    public override string ToString()
    {
      return Value.ToString("F", CultureInfo.InvariantCulture);
    }
  }
}