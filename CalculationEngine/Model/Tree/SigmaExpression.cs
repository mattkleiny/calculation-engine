using System;
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
      throw new NotImplementedException();
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