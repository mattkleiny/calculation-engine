using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class VariableExpression : CalculationExpression
  {
    public Symbol Symbol { get; }

    public VariableExpression(Symbol symbol)
    {
      Symbol = symbol;
    }

    internal override decimal Evaluate(EvaluationContext context)
    {
      return context.Variables[Symbol];
    }

    internal override Expression Compile(CompilationContext context)
    {
      return Expression.Variable(typeof(decimal), Symbol.Name);
    }

    internal override void Explain(ExplanationContext context)
    {
    }

    public override string ToString()
    {
      return Symbol.ToString();
    }
  }
}