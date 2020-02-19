using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CalculationEngine.Model.Semantics;

namespace CalculationEngine.Model
{
  [DebuggerDisplay("CalculationExplanation ({steps.Count} steps)")]
  public sealed class CalculationExplanation : IReadOnlyList<CalculationExplanation.Step>
  {
    private readonly List<Step> steps;

    public CalculationExplanation(IEnumerable<Step> steps)
    {
      this.steps = steps.ToList();
    }

    public int Count => steps.Count;

    public Step this[int index] => steps[index];
    public Step this[Index index] => steps[index];

    public List<Step> this[Range range]
    {
      get
      {
        var (offset, length) = range.GetOffsetAndLength(Count);

        return steps.GetRange(offset, length);
      }
    }

    public List<Step>.Enumerator        GetEnumerator() => steps.GetEnumerator();
    IEnumerator<Step> IEnumerable<Step>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.            GetEnumerator() => GetEnumerator();

    public sealed class Step
    {
      public string  Label       { get; }
      public string  Description { get; }
      public decimal Amount      { get; }

      public Step(IHasExplanation explanation, decimal amount)
      {
        Label       = explanation.Label;
        Description = explanation.Description;
        Amount      = amount;
      }

      public override string ToString() => $"Step {Description} ({Label})";
    }
  }
}