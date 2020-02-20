using System.Linq.Expressions;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class LabelExpression : CalculationExpression
  {
    public string                Label      { get; }
    public CalculationExpression Expression { get; }

    public LabelExpression(string label, CalculationExpression expression)
    {
      Label      = label;
      Expression = expression;
    }

    internal override decimal Evaluate(EvaluationContext context)
    {
      return Expression.Evaluate(context);
    }

    internal override void Explain(ExplanationContext context)
    {
      Expression.Explain(context);

      context.AddStep(Label, this);
    }

    internal override Expression Compile()
    {
      return Expression.Compile();
    }

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return Expression.ToString();
    }
  }
}