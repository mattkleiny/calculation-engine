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

    public UnaryExpression(UnaryOperation operation, CalculationExpression operand)
    {
      Operation = operation;
      Operand   = operand;
    }

    internal override decimal Evaluate(EvaluationContext context) => Operation switch
    {
      UnaryOperation.Negate => -Operand.Evaluate(context),
      _ => throw new ArgumentOutOfRangeException(nameof(Operation))
    };

    internal override void Explain(ExplanationContext context)
    {
      Operand.Explain(context);
    }

    internal override Expression Compile() => Operation switch
    {
      UnaryOperation.Negate => Expression.NegateChecked(Operand.Compile()),
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