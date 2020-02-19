namespace CalculationEngine.Model.Evaluation
{
  public interface IEvaluatable
  {
    decimal Evaluate(EvaluationContext context);
  }
}