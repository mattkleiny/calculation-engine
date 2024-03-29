using System.Linq.Expressions;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  /// <summary>
  /// A simplistic internal AST for modelling different types of calculations from parts.
  /// <para/>
  /// We use an AST instead of a simple data structure as this opens up possibilities in the future
  /// to transpile from different languages (C# via LINQ or Roslyn, or a new language entirely). 
  /// </summary>
  internal abstract record CalculationExpression
  {
    internal abstract decimal Evaluate(EvaluationContext context);
    internal abstract void    Explain(ExplanationContext context);

    public abstract override string ToString();

    public static implicit operator Calculation(CalculationExpression expression) => new(expression);

    /// <summary>Builds a <see cref="ParameterExpression"/> that accesses the passed <see cref="EvaluationContext"/>.</summary>
    internal static ParameterExpression ContextParameter { get; } = Expression.Parameter(typeof(EvaluationContext), "context");
  }
}