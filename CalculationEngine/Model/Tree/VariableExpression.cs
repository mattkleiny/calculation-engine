using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Tree
{
  internal sealed class VariableExpression : CalculationExpression
  {
    public Symbol Symbol { get; }

    public VariableExpression(Symbol symbol)
    {
      Symbol = symbol;
    }

    public override decimal Evaluate(EvaluationContext context)
    {
      return context.Variables[Symbol];
    }

    public override Expression Compile(CompilationContext context)
    {
      return Expression.Variable(typeof(decimal), Symbol.Name);
    }

    public override void Explain(ExplanationContext context)
    {
    }

    public override string ToString()
    {
      return Symbol.ToString();
    }
  }
}