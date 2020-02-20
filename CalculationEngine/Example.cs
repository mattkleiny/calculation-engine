using System;
using CalculationEngine.Model.Evaluation;
using static CalculationEngine.Model.Calculation;
using static CalculationEngine.Model.Evaluation.EarningsCategory;
using static CalculationEngine.Model.Evaluation.TaxCategory;

namespace CalculationEngine
{
  public static class Example
  {
    public static void Main()
    {
      var earnings   = YTD(Earnings);
      var allowances = YTD(Allowances);
      var deductions = YTD(Deductions);
      var leave      = YTD(Leave);

      var allEarnings = earnings + allowances + deductions + leave;
      var total       = Round(earnings - allowances - deductions - leave) + Truncate(Tax(PAYG, allEarnings));

      var output = total.Evaluate(new EvaluationContext());

      Console.WriteLine($"{total} = {output}");
    }
  }
}