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
  /// <para/>
  /// Each expression is individually responsible for self-evaluation, compilation, explanation.
  /// However, there is a simple <see cref="ICalculationVisitor{T}"/> pattern if you need to walk
  /// the entire tree. 
  /// </summary>
  internal abstract class CalculationExpression
  {
    internal abstract decimal    Evaluate(EvaluationContext context);
    internal abstract void       Explain(ExplanationContext context);
    internal abstract Expression Compile();

    internal abstract T Accept<T>(ICalculationVisitor<T> visitor);

    public abstract override string ToString();

    public static implicit operator Calculation(CalculationExpression expression) => new Calculation(expression);
  }
}