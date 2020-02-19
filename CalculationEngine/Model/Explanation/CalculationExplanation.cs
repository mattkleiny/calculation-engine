using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CalculationEngine.Model.Explanation
{
  [DebuggerDisplay("CalculationExplanation ({steps.Count} steps)")]
  public sealed class CalculationExplanation : IReadOnlyList<CalculationStep>
  {
    private readonly List<CalculationStep> steps;

    public CalculationExplanation(IEnumerable<CalculationStep> steps)
    {
      this.steps = steps.ToList();
    }

    public int Count => steps.Count;

    public CalculationStep this[int index] => steps[index];
    public CalculationStep this[Index index] => steps[index];

    public List<CalculationStep> this[Range range]
    {
      get
      {
        var (offset, length) = range.GetOffsetAndLength(Count);

        return steps.GetRange(offset, length);
      }
    }

    public List<CalculationStep>.Enumerator                   GetEnumerator() => steps.GetEnumerator();
    IEnumerator<CalculationStep> IEnumerable<CalculationStep>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.                                  GetEnumerator() => GetEnumerator();
  }
}