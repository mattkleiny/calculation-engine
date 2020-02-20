using System.Linq.Expressions;
using System.Reflection;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  // TODO: abstract this into a 'ClosedExpression' to permit automatic evaluation/compilation from a shared method.

  internal sealed class TaxExpression : CalculationExpression
  {
    public TaxCategory           Category { get; }
    public CalculationExpression Value    { get; }

    public TaxExpression(TaxCategory category, CalculationExpression value)
    {
      Category = category;
      Value    = value;
    }

    internal override decimal Evaluate(EvaluationContext context)
    {
      var cumulative = Value.Evaluate(context);

      return EvaluateTax(context, Category, cumulative);
    }

    internal override void Explain(ExplanationContext context)
    {
      Value.Explain(context);
    }

    internal override Expression Compile()
    {
      var method = typeof(TaxExpression).GetMethod(nameof(EvaluateTax), BindingFlags.Static | BindingFlags.NonPublic);

      var context    = ContextParameter;
      var category   = Expression.Constant(Category);
      var cumulative = Value.Compile();

      return Expression.Call(null, method, new[] { context, category, cumulative });
    }

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"(Calculate {Category} Tax {Value})";
    }

    private static decimal EvaluateTax(EvaluationContext context, TaxCategory category, decimal cumulative)
    {
      var table = context.TaxTables[category];
      var (a, b) = table[cumulative];

      return a * cumulative - b;
    }
  }
}