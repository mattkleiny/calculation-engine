using System;
using System.Linq.Expressions;
using CalculationEngine.Model.AST;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using CalculationEngine.Model.Parsing.LINQ;
using CalculationEngine.Model.Parsing.Roslyn;

namespace CalculationEngine.Model
{
  // TODO: add a fluent 'builder' pattern on top of this

  public sealed class Calculation
  {
    private readonly CalculationExpression expression;

    public static Calculation Parse(string path)                          => new Calculation(RoslynParser.Parse(path));
    public static Calculation Parse(Expression<Func<decimal>> expression) => new Calculation(LinqParser.Parse(expression));

    internal Calculation(CalculationExpression expression)
    {
      this.expression = expression;
    }

    public decimal Evaluate(EvaluationContext context)
    {
      return expression.Evaluate(context);
    }

    public Func<decimal> Compile()
    {
      var context    = new CompilationContext();
      var tree       = expression.Compile(context);
      var invocation = Expression.Lambda(tree).Compile();

      return () => (decimal) invocation.DynamicInvoke();
    }

    public CalculationExplanation Explain(EvaluationContext context)
    {
      var explanation = new ExplanationContext(context);

      expression.Explain(explanation);

      return new CalculationExplanation(explanation.Steps);
    }

    public override string ToString() => expression.ToString();
  }
}