using CalculationEngine.Model.Evaluation;

namespace CalculationEngine.Model.Semantics
{
  public interface IEvaluatable
  {
    decimal Evaluate(CalculationContext context);
  }
}