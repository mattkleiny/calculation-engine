using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CalculationEngine.Model.AST;

namespace CalculationEngine.Model
{
  [DebuggerDisplay("CalculationExplanation ({steps.Count} steps)")]
  public sealed class CalculationExplanation : IEnumerable<CalculationExplanation.Step>
  {
    private readonly List<Step> steps;

    public CalculationExplanation(IEnumerable<Step> steps)
    {
      this.steps = steps.ToList();
    }

    public List<Step>.Enumerator        GetEnumerator() => steps.GetEnumerator();
    IEnumerator<Step> IEnumerable<Step>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.            GetEnumerator() => GetEnumerator();

    [DebuggerDisplay("Step {Operation} ({Label})")]
    public sealed class Step
    {
      public CalculationExpression Expression { get; }
      public string                Label      { get; }
      public string                Operation  { get; }
      public decimal               Amount     { get; }

      public Step(CalculationExpression expression)
      {
        Expression = expression;
        Label      = expression.Label;
        Operation  = expression.ToString();
        Amount     = expression.Evaluate();
      }
    }
  }
}