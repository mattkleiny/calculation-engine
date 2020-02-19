using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Nodes
{
  public abstract class CalculationExpression
  {
    internal abstract decimal    Evaluate(EvaluationContext context);
    internal abstract Expression Compile(CompilationContext context);
    internal abstract void       Explain(ExplanationContext context);

    internal abstract T Accept<T>(IVisitor<T> visitor);

    public abstract override string ToString();
  }
}