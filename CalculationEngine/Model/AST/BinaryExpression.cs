using System;
using CalculationEngine.Model.Evaluation;

namespace CalculationEngine.Model.AST
{
  public sealed class BinaryExpression : CalculationExpression
  {
    public BinaryOperation       Operation { get; }
    public CalculationExpression Left      { get; }
    public CalculationExpression Right     { get; }

    public BinaryExpression(BinaryOperation operation, CalculationExpression left, CalculationExpression right, string label = null)
    {
      Operation = operation;
      Left      = left;
      Right     = right;
      Label     = label ?? string.Empty;
    }

    public override decimal Evaluate(CalculationContext context) => Operation switch
    {
      BinaryOperation.Add => Left.Evaluate(context) + Right.Evaluate(context),
      BinaryOperation.Subtract => Left.Evaluate(context) - Right.Evaluate(context),
      BinaryOperation.Multiply => Left.Evaluate(context) * Right.Evaluate(context),
      BinaryOperation.Divide => Left.Evaluate(context) / Right.Evaluate(context),
      _ => throw new ArgumentOutOfRangeException(nameof(Operation))
    };

    public override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"({Left} {ConvertToString(Operation)} {Right})";
    }

    private static string ConvertToString(BinaryOperation operation) => operation switch
    {
      BinaryOperation.Add => "+",
      BinaryOperation.Subtract => "-",
      BinaryOperation.Multiply => "*",
      BinaryOperation.Divide => "/",
      _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
    };
  }
}