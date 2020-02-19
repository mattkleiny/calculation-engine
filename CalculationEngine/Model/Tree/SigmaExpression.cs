using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Tree
{
  internal sealed class SigmaExpression : CalculationExpression
  {
    public CalculationExpression[] Expressions { get; }

    public SigmaExpression(params CalculationExpression[] expressions)
    {
      Expressions = expressions;
    }

    public override decimal Evaluate(EvaluationContext context)
    {
      return Expressions.Sum(expression => expression.Evaluate(context));
    }

    public override Expression Compile(CompilationContext context)
    {
      var queue = new Queue<Expression>();

      foreach (var expression in Expressions)
      {
        queue.Enqueue(expression.Compile(context));
      }

      var latest = queue.Dequeue();

      while (queue.Count > 0)
      {
        latest = Expression.AddChecked(latest, queue.Dequeue());
      }

      return latest;
    }

    public override void Explain(ExplanationContext context)
    {
      context.Steps.Add(new CalculationStep("Σ", ToString(), Evaluate(context.Evaluation)));
    }

    public override string ToString()
    {
      return $"(Σ {string.Join(" + ", Expressions.Select(_ => _.ToString()))})";
    }
  }
}