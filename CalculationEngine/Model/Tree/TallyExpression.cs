using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Tree
{
  internal sealed class TallyExpression : CalculationExpression
  {
    public EarningsCategory Category { get; }

    public TallyExpression(EarningsCategory category)
    {
      Category = category;
    }

    public override decimal Evaluate(EvaluationContext context)
    {
      // TODO: sample this from somewhere on the context
      return 100m;
    }

    public override Expression Compile(CompilationContext context)
    {
      // TODO: sample this from somewhere on the context
      return Expression.Constant(100m);
    }

    public override void Explain(ExplanationContext context)
    {
      context.Steps.Add(new CalculationStep($"Tally {Category} YTD", ToString(), Evaluate(context.Evaluation)));
    }

    public override string ToString()
    {
      return $"(YTD {Category})";
    }
  }
}