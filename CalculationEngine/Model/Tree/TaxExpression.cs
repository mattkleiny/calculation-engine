using System;
using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Tree
{
  internal sealed class TaxExpression : CalculationExpression
  {
    public TaxOperation          Mode     { get; }
    public TaxCategory           Category { get; }
    public CalculationExpression Value    { get; }
    public string                Label    { get; }

    public TaxExpression(TaxOperation mode, TaxCategory category, CalculationExpression value, string label = null)
    {
      Mode     = mode;
      Category = category;
      Value    = value;
      Label    = label ?? string.Empty;
    }

    public override decimal Evaluate(EvaluationContext context)
    {
      var table = context.TaxTables[Category];

      var amount    = Value.Evaluate(context);
      var taxAmount = amount * table[amount];

      return Mode switch
      {
        TaxOperation.Calculate => taxAmount,
        TaxOperation.Add => amount + taxAmount,
        TaxOperation.Subtract => amount - taxAmount,
        _ => throw new ArgumentOutOfRangeException(nameof(Mode))
      };
    }

    public override Expression Compile(CompilationContext context)
    {
      // TODO: evaluate this on the tax table
      return Expression.MultiplyChecked(Value.Compile(context), Expression.Constant(0.20m));
    }

    public override void Explain(ExplanationContext context)
    {
      Value.Explain(context);

      if (!string.IsNullOrEmpty(Label))
      {
        context.Steps.Add(new CalculationStep(Label, ToString(), Evaluate(context.Evaluation)));
      }
    }

    public override string ToString()
    {
      return $"({Mode} {Category} Tax {Value})";
    }
  }
}