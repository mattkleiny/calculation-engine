using System.Linq.Expressions;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model.Tree
{
  internal abstract class CalculationExpression : IEvaluatable, ICompilable, IExplainable
  {
    public abstract decimal    Evaluate(EvaluationContext context);
    public abstract Expression Compile(CompilationContext context);
    public abstract void       Explain(ExplanationContext context);

    public abstract override string ToString();
  }
}