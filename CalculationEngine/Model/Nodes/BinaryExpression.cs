using System;
using System.Linq.Expressions;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class BinaryExpression : CalculationExpression
  {
    public BinaryOperation       Operation { get; }
    public CalculationExpression Left      { get; }
    public CalculationExpression Right     { get; }

    public BinaryExpression(BinaryOperation operation, CalculationExpression left, CalculationExpression right)
    {
      Operation = operation;
      Left      = left;
      Right     = right;
    }

    internal override decimal Evaluate(EvaluationContext context) => Operation switch
    {
      BinaryOperation.Add => Left.Evaluate(context) + Right.Evaluate(context),
      BinaryOperation.Subtract => Left.Evaluate(context) - Right.Evaluate(context),
      BinaryOperation.Multiply => Left.Evaluate(context) * Right.Evaluate(context),
      BinaryOperation.Divide => Left.Evaluate(context) / Right.Evaluate(context),
      _ => throw new ArgumentOutOfRangeException(nameof(Operation))
    };

    internal override void Explain(ExplanationContext context)
    {
      Left.Explain(context);
      Right.Explain(context);
    }

    internal override Expression Compile() => Operation switch
    {
      BinaryOperation.Add => Expression.AddChecked(Left.Compile(), Right.Compile()),
      BinaryOperation.Subtract => Expression.SubtractChecked(Left.Compile(), Right.Compile()),
      BinaryOperation.Multiply => Expression.MultiplyChecked(Left.Compile(), Right.Compile()),
      BinaryOperation.Divide => Expression.Divide(Left.Compile(), Right.Compile()),
      _ => throw new ArgumentOutOfRangeException(nameof(Operation))
    };

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
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