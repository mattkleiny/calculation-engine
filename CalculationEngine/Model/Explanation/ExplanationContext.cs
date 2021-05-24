using System.Collections.Generic;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Nodes;

namespace CalculationEngine.Model.Explanation
{
  internal sealed class ExplanationContext
  {
    private readonly List<CalculationExplanation.Step> steps = new();
    private readonly EvaluationContext                 context;

    public ExplanationContext(EvaluationContext context)
    {
      this.context = context;
    }

    public void AddStep(string label, CalculationExpression expression)
    {
      var description = expression.ToString();
      var amount      = expression.Evaluate(context);

      steps.Add(new CalculationExplanation.Step(label, description, amount));
    }

    public CalculationExplanation ToExplanation() => new(steps);
  }
}