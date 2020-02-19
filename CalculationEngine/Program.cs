using System;
using CalculationEngine.Model;
using CalculationEngine.Model.Evaluation;
using static CalculationEngine.Model.Evaluation.EarningsCategory;
using static CalculationEngine.Model.Evaluation.Operations;
using static CalculationEngine.Model.Evaluation.TaxCategory;

namespace CalculationEngine
{
  public static class Program
  {
    public static void Main()
    {
      // TODO: perhaps make it possible just to directly transliterate this guy into a series of nodes?

      var graph       = Calculation.Parse(() => Tax(PAYG, YTD(Earnings) + YTD(Allowances) + YTD(Deductions) + YTD(Leave)) / 16m);
      var context     = new EvaluationContext();
      var explanation = graph.Explain(context);

      Console.WriteLine("Steps: ");
      foreach (var step in explanation)
      {
        Console.WriteLine(step.ToString());
      }

      Console.Write("Calculation: ");
      Console.WriteLine(graph.ToString());

      Console.Write("Output: ");
      Console.WriteLine(graph.Evaluate(context));
    }
  }
}