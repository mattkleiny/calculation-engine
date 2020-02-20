using System.Linq.Expressions;
using System.Reflection;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using CalculationEngine.Model.Utilities;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class TallyExpression : CalculationExpression
  {
    public EarningsCategory Categories { get; }

    public TallyExpression(EarningsCategory categories)
    {
      Categories = categories;
    }

    internal override decimal Evaluate(EvaluationContext context)
    {
      return EvaluateTally(context, Categories);
    }

    internal override void Explain(ExplanationContext context)
    {
    }

    internal override Expression Compile()
    {
      var method = typeof(TallyExpression).GetMethod(nameof(EvaluateTally), BindingFlags.Static | BindingFlags.NonPublic);

      var context    = ContextParameter;
      var categories = Expression.Constant(Categories);

      return Expression.Call(null, method, context, categories);
    }

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"(YTD {Categories.ToPermutationString()})";
    }

    private static decimal EvaluateTally(EvaluationContext context, EarningsCategory categories)
    {
      return context.Earnings.SumYearToDate(categories);
    }
  }
}