using System;
using System.Linq.Expressions;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  internal sealed class UnaryExpression : CalculationExpression
  {
    public UnaryOperation        Operation { get; }
    public CalculationExpression Operand   { get; }
    public string                Label     { get; }

    public UnaryExpression(UnaryOperation operation, CalculationExpression operand, string label = null)
    {
      Operation = operation;
      Operand   = operand;
      Label     = label ?? string.Empty;
    }

    internal override decimal Evaluate(EvaluationContext context) => Operation switch
    {
      UnaryOperation.Negate => -Operand.Evaluate(context),
      _ => throw new ArgumentOutOfRangeException(nameof(Operation))
    };

    internal override void Explain(ExplanationContext context)
    {
      if (!string.IsNullOrEmpty(Label))
      {
        context.AddStep(Label, this);
      }
    }

    internal override Expression Compile() => Operation switch
    {
      UnaryOperation.Negate => Expression.Not(Operand.Compile()),
      _ => throw new ArgumentOutOfRangeException(nameof(Operation))
    };

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