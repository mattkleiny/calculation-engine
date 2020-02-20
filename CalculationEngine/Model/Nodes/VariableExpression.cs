using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class VariableExpression : CalculationExpression
  {
    public Symbol                Symbol       { get; }
    public CalculationExpression Operand      { get; }
    public bool                  IncludeLabel { get; }

    public VariableExpression(Symbol symbol, CalculationExpression operand, bool includeLabel)
    {
      Symbol       = symbol;
      Operand      = operand;
      IncludeLabel = includeLabel;
    }

    internal override decimal Evaluate(EvaluationContext context)
    {
      return EvaluateVariable(context, Symbol, () => Operand.Evaluate(context));
    }

    internal override void Explain(ExplanationContext context)
    {
      Operand.Explain(context);

      if (IncludeLabel)
      {
        context.AddStep(Symbol.ToString(), this);
      }
    }

    internal override Expression Compile()
    {
      var method = typeof(VariableExpression).GetMethod(nameof(EvaluateVariable), BindingFlags.Static | BindingFlags.NonPublic);

      var context = ContextParameter;
      var symbol  = Expression.Constant(Symbol);
      var factory = Expression.Lambda<Func<decimal>>(Operand.Compile(), Enumerable.Empty<ParameterExpression>());

      return Expression.Call(null, method, new Expression[] { context, symbol, factory });
    }

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"({Symbol} = {Operand})";
    }

    private static decimal EvaluateVariable(EvaluationContext context, Symbol symbol, Func<decimal> factory)
    {
      return context.Results.GetOrCompute(symbol.ToString(), factory);
    }
  }
}