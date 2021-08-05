using System;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal enum BinaryOperation
  {
    Add,
    Subtract,
    Multiply,
    Divide,
    Modulo
  }

  internal sealed record BinaryExpression(BinaryOperation Operation, CalculationExpression Left, CalculationExpression Right) : CalculationExpression
  {
    internal override decimal Evaluate(EvaluationContext context) => Operation switch
    {
      BinaryOperation.Add      => Left.Evaluate(context) + Right.Evaluate(context),
      BinaryOperation.Subtract => Left.Evaluate(context) - Right.Evaluate(context),
      BinaryOperation.Multiply => Left.Evaluate(context) * Right.Evaluate(context),
      BinaryOperation.Divide   => Left.Evaluate(context) / Right.Evaluate(context),
      BinaryOperation.Modulo   => Left.Evaluate(context) % Right.Evaluate(context),

      _ => throw new ArgumentOutOfRangeException(nameof(Operation))
    };

    internal override void Explain(ExplanationContext context)
    {
      Left.Explain(context);
      Right.Explain(context);
    }

    public override string ToString()
    {
      return $"({Left} {ConvertToString(Operation)} {Right})";
    }

    private static string ConvertToString(BinaryOperation operation) => operation switch
    {
      BinaryOperation.Add      => "+",
      BinaryOperation.Subtract => "-",
      BinaryOperation.Multiply => "*",
      BinaryOperation.Divide   => "/",
      BinaryOperation.Modulo   => "Mod",

      _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
    };
  }
}