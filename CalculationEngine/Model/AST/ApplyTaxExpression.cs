using System;
using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.AST
{
  public sealed class ApplyTaxExpression : CalculationExpression
  {
    public TaxCategory           Category { get; }
    public CalculationExpression Value    { get; }
    public string                Label    { get; }

    public ApplyTaxExpression(TaxCategory category, CalculationExpression value, string label = null)
    {
      Category = category;
      Value    = value;
      Label    = label ?? string.Empty;
    }

    public override decimal Evaluate(EvaluationContext context)
    {
      var table  = context.TaxTables[Category];
      var amount = Value.Evaluate(context);

      return amount * table[amount];
    }

    public override Expression Compile(CompilationContext context)
    {
      throw new NotImplementedException();
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
      return $"({Category} Tax {Value})";
    }
  }
}