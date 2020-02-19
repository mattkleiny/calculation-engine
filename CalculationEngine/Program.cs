using System;
using CalculationEngine.Model.Evaluation;
using static CalculationEngine.Model.Calculation;
using static CalculationEngine.Model.Evaluation.EarningsCategory;
using static CalculationEngine.Model.Evaluation.TaxCategory;

namespace CalculationEngine
{
  public static class Program
  {
    public static void Main()
    {
      var gross = Σ(YTD(Earnings), YTD(Allowances), YTD(Deductions), YTD(Leave));
      var net   = gross - Tax(PAYG, gross);

      var output = net.Evaluate(new EvaluationContext());

      Console.WriteLine($"{net} = {output}");
    }
  }
}