using System.Linq;
using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class BlockExpression : CalculationExpression
  {
    public CalculationExpression[] Expressions { get; }

    public BlockExpression(params CalculationExpression[] expressions)
    {
      Expressions = expressions;
    }

    internal override decimal Evaluate(EvaluationContext context)
    {
      if (Expressions.Length == 0)
      {
        return 0m;
      }

      if (Expressions.Length == 1)
      {
        return Expressions[0].Evaluate(context);
      }

      foreach (var expression in Expressions[..^2])
      {
        expression.Evaluate(context);
      }

      return Expressions[^1].Evaluate(context);
    }

    internal override Expression Compile(CompilationContext context)
    {
      return Expression.Block(Expressions.Select(_ => _.Compile(context)));
    }

    internal override void Explain(ExplanationContext context)
    {
      foreach (var expression in Expressions)
      {
        expression.Explain(context);
      }
    }

    public override string ToString()
    {
      return $"({string.Join(",", Expressions.Select(_ => _.ToString()))})";
    }
  }
}