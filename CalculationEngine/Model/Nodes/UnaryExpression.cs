using System;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using CalculationEngine.Model.Visitors;

namespace CalculationEngine.Model.Nodes
{
  internal enum UnaryOperation
  {
    Negate
  }

  internal sealed record UnaryExpression(UnaryOperation Operation, CalculationExpression Operand) : CalculationExpression
  {
    internal override decimal Evaluate(EvaluationContext context) => Operation switch
    {
      UnaryOperation.Negate => -Operand.Evaluate(context),

      _ => throw new ArgumentOutOfRangeException(nameof(Operation))
    };

    internal override void Explain(ExplanationContext context)
    {
      Operand.Explain(context);
    }

    public override string ToString()
    {
      return $"({ConvertToString(Operation)} {Operand})";
    }

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    private static string ConvertToString(UnaryOperation operation) => operation switch
    {
      UnaryOperation.Negate => "-",

      _ => throw new ArgumentOutOfRangeException(nameof(operation))
    };
  }
}