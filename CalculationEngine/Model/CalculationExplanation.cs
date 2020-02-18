using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Model
{
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

    public sealed class Step
    {
      public string Label       { get; }
      public string Description { get; }

      public Step(string label, string description)
      {
        Label       = label;
        Description = description;
      }
    }
  }
}