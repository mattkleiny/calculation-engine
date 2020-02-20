using System.Linq.Expressions;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  /// <summary>
  /// A simplistic internal AST for modelling different types of calculations from parts.
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