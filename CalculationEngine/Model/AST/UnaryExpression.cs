using System;
using CalculationEngine.Model.Evaluation;

namespace CalculationEngine.Model.AST
{
  public sealed class UnaryExpression : CalculationExpression
  {
    public UnaryOperation        Operation { get; }
    public CalculationExpression Operand   { get; }

    public UnaryExpression(UnaryOperation operation, CalculationExpression operand, string label = null)
    {
      Operation = operation;
      Operand   = operand;
      Label     = label ?? string.Empty;
    }

    public override decimal Evaluate(CalculationContext context) => Operation switch
    {
      UnaryOperation.Not => -Operand.Evaluate(context),
      _ => throw new ArgumentOutOfRangeException(nameof(Operation))
    };

    public override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"({ConvertToString(Operation)} {Operand})";
    }

    private static string ConvertToString(UnaryOperation operation) => operation switch
    {
      UnaryOperation.Not => "-",
      _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
    };
  }
}