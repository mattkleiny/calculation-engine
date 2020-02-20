using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class SigmaExpression : CalculationExpression
  {
    public CalculationExpression[] Expressions { get; }

    public SigmaExpression(IEnumerable<CalculationExpression> expressions)
    {
      Expressions = expressions.ToArray();
    }

    internal override decimal Evaluate(EvaluationContext context)
    {
      return Expressions.Sum(expression => expression.Evaluate(context));
    }

    internal override void Explain(ExplanationContext context)
    {
      foreach (var expression in Expressions)
      {
        expression.Explain(context);
      }
    }

    internal override Expression Compile()
    {
      // build a sequence of binary expressions that add the given elements sequentially
      var queue = new Queue<Expression>();

      foreach (var expression in Expressions)
      {
        queue.Enqueue(expression.Compile());
      }

      var latest = queue.Dequeue();

      while (queue.Count > 0)
      {
        latest = Expression.AddChecked(latest, queue.Dequeue());
      }

      return latest;
    }

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"(Σ {string.Join(" + ", Expressions.Select(_ => _.ToString()))})";
    }
  }
}