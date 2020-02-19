using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class TaxExpression : CalculationExpression
  {
    public TaxCategory           Category { get; }
    public CalculationExpression Value    { get; }
    public string                Label    { get; }

    public TaxExpression(TaxCategory category, CalculationExpression value, string label = null)
    {
      Category = category;
      Value    = value;
      Label    = label ?? string.Empty;
    }

    internal override decimal Evaluate(EvaluationContext context)
    {
      var table  = context.TaxTables[Category];
      var amount = Value.Evaluate(context);

      return amount * table[amount];
    }

    internal override Expression Compile(CompilationContext context)
    {
      // TODO: evaluate this on the tax table
      return Expression.MultiplyChecked(Value.Compile(context), Expression.Constant(0.20m));
    }

    internal override void Explain(ExplanationContext context)
    {
      Value.Explain(context);

      if (!string.IsNullOrEmpty(Label))
      {
        context.Steps.Add(new CalculationStep(Label, ToString(), Evaluate(context.Evaluation)));
      }
    }

    internal override T Accept<T>(IVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"(Calculate {Category} Tax {Value})";
    }
  }
}