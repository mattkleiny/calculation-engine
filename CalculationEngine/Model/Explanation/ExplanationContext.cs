using System.Collections.Generic;
using CalculationEngine.Model.Evaluation;

namespace CalculationEngine.Model.Explanation
{
  internal sealed class ExplanationContext
  {
    public List<CalculationStep> Steps      { get; } = new List<CalculationStep>();
    public EvaluationContext     Evaluation { get; }

    public ExplanationContext(EvaluationContext evaluation)
    {
      Evaluation = evaluation;
    }
  }
}