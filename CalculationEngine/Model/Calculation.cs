using System;
using System.Linq;
using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using CalculationEngine.Model.Nodes;
using BinaryExpression = CalculationEngine.Model.Nodes.BinaryExpression;
using ConstantExpression = CalculationEngine.Model.Nodes.ConstantExpression;
using UnaryExpression = CalculationEngine.Model.Nodes.UnaryExpression;

namespace CalculationEngine.Model
{
  public readonly struct Calculation
  {
    public static Calculation Σ(params Calculation[] expressions)               => new SigmaExpression(expressions.Select(_ => _.expression).ToArray());
    public static Calculation Tax(TaxCategory category, Calculation expression) => new TaxExpression(category, expression);
    public static Calculation YTD(EarningsCategory category)                    => new TallyExpression(category);

    private readonly CalculationExpression expression;

    private Calculation(CalculationExpression expression)
    {
      this.expression = expression;
    }

    public decimal Evaluate(EvaluationContext context)
    {
      return expression.Evaluate(context);
    }

    public Func<EvaluationContext, decimal> Compile()
    {
      var context    = new CompilationContext();
      var tree       = expression.Compile(context);
      var invocation = Expression.Lambda(tree).Compile();

      return _ => (decimal) invocation.DynamicInvoke();
    }

    public CalculationExplanation Explain(EvaluationContext context)
    {
      var explanation = new ExplanationContext(context);

      expression.Explain(explanation);

      return new CalculationExplanation(explanation.Steps);
    }

    public override string ToString()
    {
      return expression.ToString();
    }
    
    public static implicit operator Calculation(decimal value)                    => new Calculation(new ConstantExpression(value));
    public static implicit operator Calculation(CalculationExpression expression) => new Calculation(expression);
    public static implicit operator CalculationExpression(Calculation expression) => expression.expression;

    public static Calculation operator -(Calculation a)                => new UnaryExpression(UnaryOperation.Not, a.expression);
    public static Calculation operator +(Calculation a, Calculation b) => new BinaryExpression(BinaryOperation.Add, a.expression, b.expression);
    public static Calculation operator -(Calculation a, Calculation b) => new BinaryExpression(BinaryOperation.Subtract, a.expression, b.expression);
    public static Calculation operator *(Calculation a, Calculation b) => new BinaryExpression(BinaryOperation.Multiply, a.expression, b.expression);
    public static Calculation operator /(Calculation a, Calculation b) => new BinaryExpression(BinaryOperation.Divide, a.expression, b.expression);
  }
}