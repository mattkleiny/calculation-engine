using System;
using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.AST
{
  public sealed class UnaryExpression : CalculationExpression
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

    public override decimal Evaluate(EvaluationContext context) => Operation switch
    {
      UnaryOperation.Not => -Operand.Evaluate(context),
      _ => throw new ArgumentOutOfRangeException(nameof(Operation))
    };

    public override Expression Compile(CompilationContext context) => Operation switch
    {
      UnaryOperation.Not => Expression.Not(Operand.Compile(context)),
      _ => throw new ArgumentOutOfRangeException(nameof(Operation))
    };

    public override void Explain(ExplanationContext context)
    {
      if (!string.IsNullOrEmpty(Label))
      {
        context.Steps.Add(new CalculationStep(Label, ToString(), Evaluate(context.Evaluation)));
      }
    }

    public override string ToString()
    {
      return $"({ConvertToString(Operation)} {Operand})";
    }

    private static string ConvertToString(UnaryOperation operation) => operation switch
    {
      UnaryOperation.Not => "-",
      _ => throw new ArgumentOutOfRangeException(nameof(operation))
    };
  }
}