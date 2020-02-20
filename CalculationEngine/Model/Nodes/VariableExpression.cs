using System.Linq.Expressions;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class VariableExpression : CalculationExpression
  {
    public Symbol                Symbol       { get; }
    public CalculationExpression Expression   { get; }
    public bool                  IncludeLabel { get; }

    public VariableExpression(Symbol symbol, CalculationExpression expression, bool includeLabel)
    {
      Symbol       = symbol;
      Expression   = expression;
      IncludeLabel = includeLabel;
    }

    internal override decimal Evaluate(EvaluationContext context)
    {
      return context.Results.GetOrCompute(Symbol.ToString(), () => Expression.Evaluate(context));
    }

    internal override void Explain(ExplanationContext context)
    {
      Expression.Explain(context);

      if (IncludeLabel)
      {
        context.AddStep(Symbol.ToString(), this);
      }
    }

    internal override Expression Compile()
    {
      // TODO: use a functional environment pattern to pass the EvaluationContext down here in the expression tree.

      return Expression.Compile();
    }

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"({Symbol} = {Expression})";
    }
  }
}