using System;

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

    public override decimal Evaluate() => Operation switch
    {
      BinaryOperation.Add => Left.Evaluate() + Right.Evaluate(),
      BinaryOperation.Subtract => Left.Evaluate() - Right.Evaluate(),
      BinaryOperation.Multiply => Left.Evaluate() * Right.Evaluate(),
      BinaryOperation.Divide => Left.Evaluate() / Right.Evaluate(),

      _ => throw new ArgumentOutOfRangeException(nameof(Operation))
    };

    public override T      Accept<T>(CalculationVisitor<T> visitor) => visitor.Visit(this);
    public override string ToString()                               => $"({Left} {ConvertToString(Operation)} {Right})";

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