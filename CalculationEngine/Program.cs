using System;
using CalculationEngine.Model;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Tree;
using static CalculationEngine.Model.Evaluation.EarningsCategory;
using static CalculationEngine.Model.Evaluation.FluentCalculations;
using static CalculationEngine.Model.Evaluation.TaxCategory;

namespace CalculationEngine
{
  public static class Program
  {
    public static void Main()
    {
      // TODO: perhaps make it possible just to directly transliterate this guy into a series of nodes?

      CalculationExpression calculation = Tax(PAYG, YTD(Earnings) + YTD(Allowances) + YTD(Deductions) + YTD(Leave)) / 16m;

      var graph       = new Calculation(calculation);
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