using System;
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
      return 100m; // TODO: sample this from somewhere on the context
    }

    public override Expression Compile(CompilationContext context)
    {
      throw new NotImplementedException();
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