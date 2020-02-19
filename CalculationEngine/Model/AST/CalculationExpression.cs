using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Semantics;

namespace CalculationEngine.Model.AST
{
  public abstract class CalculationExpression : IEvaluatable, IHasExplanation
  {
    public string Label       { get; protected set; }
    public string Description => ToString();

    public abstract decimal Evaluate(CalculationContext context);

    public abstract T Accept<T>(ICalculationVisitor<T> visitor);
  }
}