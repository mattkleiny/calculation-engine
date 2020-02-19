using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class AssignmentExpression : CalculationExpression
  {
    public Symbol                Symbol  { get; }
    public CalculationExpression Operand { get; }

    public AssignmentExpression(Symbol symbol, CalculationExpression operand)
    {
      Symbol  = symbol;
      Operand = operand;
    }

    internal override decimal Evaluate(EvaluationContext context)
    {
      var amount = Operand.Evaluate(context);

      context.Variables[Symbol] = amount;

      return amount;
    }

    internal override Expression Compile(CompilationContext context)
    {
      return Expression.Assign(
        Expression.Variable(typeof(decimal), Symbol.Name),
        Operand.Compile(context)
      );
    }

    internal override void Explain(ExplanationContext context)
    {
      Operand.Explain(context);
    }

    public override string ToString()
    {
      return $"({Symbol} = {Operand})";
    }
  }
}