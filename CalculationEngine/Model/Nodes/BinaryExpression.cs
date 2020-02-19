using System;
using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class BinaryExpression : CalculationExpression
  {
    public BinaryOperation       Operation { get; }
    public CalculationExpression Left      { get; }
    public CalculationExpression Right     { get; }
    public string                Label     { get; }

    public BinaryExpression(BinaryOperation operation, CalculationExpression left, CalculationExpression right, string label = null)
    {
      Operation = operation;
      Left      = left;
      Right     = right;
      Label     = label ?? string.Empty;
    }

    internal override decimal Evaluate(EvaluationContext context) => Operation switch
    {
      BinaryOperation.Add => Left.Evaluate(context) + Right.Evaluate(context),
      BinaryOperation.Subtract => Left.Evaluate(context) - Right.Evaluate(context),
      BinaryOperation.Multiply => Left.Evaluate(context) * Right.Evaluate(context),
      BinaryOperation.Divide => Left.Evaluate(context) / Right.Evaluate(context),
      _ => throw new ArgumentOutOfRangeException(nameof(Operation))
    };

    internal override Expression Compile(CompilationContext context) => Operation switch
    {
      BinaryOperation.Add => Expression.AddChecked(Left.Compile(context), Right.Compile(context)),
      BinaryOperation.Subtract => Expression.SubtractChecked(Left.Compile(context), Right.Compile(context)),
      BinaryOperation.Multiply => Expression.MultiplyChecked(Left.Compile(context), Right.Compile(context)),
      BinaryOperation.Divide => Expression.Divide(Left.Compile(context), Right.Compile(context)),
      _ => throw new ArgumentOutOfRangeException(nameof(Operation))
    };

    internal override void Explain(ExplanationContext context)
    {
      Left.Explain(context);
      Right.Explain(context);

      if (!string.IsNullOrEmpty(Label))
      {
        context.Steps.Add(new CalculationStep(Label, ToString(), Evaluate(context.Evaluation)));
      }
    }

    internal override T Accept<T>(IVisitor<T> visitor)
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