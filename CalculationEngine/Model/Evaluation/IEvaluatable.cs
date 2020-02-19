namespace CalculationEngine.Model.Evaluation
{
  internal interface IEvaluatable
  {
    decimal Evaluate(EvaluationContext context);
  }
}