using System;
using CalculationEngine.Model;
using CalculationEngine.Model.Evaluation;
using static CalculationEngine.Model.Calculation;
using static CalculationEngine.Model.Evaluation.EarningsCategory;
using static CalculationEngine.Model.Evaluation.TaxCategory;

namespace CalculationEngine
{
  public static class ExampleProgram
  {
    public static Calculation Calculation { get; } = Create(() =>
    {
      var ordinaryEarnings = YTD(OrdinaryEarnings);
      var allowances       = YTD(Allowances);
      var deductions       = YTD(Deductions);
      var leave            = YTD(Leave);

      var a = Sum(ordinaryEarnings, allowances, deductions, leave);

      var b = Variable("B", Round(allowances));
      var c = Variable("C", Round(deductions));
      var d = Variable("D", Round(leave));

      var e = Variable("E", Truncate(Tax(PAYG, a)));

      return a - (b + c + d) - e / 2m;
    });

    public static void Main()
    {
      var context = new EvaluationContext();

      var output      = Calculation.Evaluate(context);
      var explanation = Calculation.Explain(context);

      Console.WriteLine($"Total: {output}");

      Console.WriteLine();
      Console.WriteLine("Explanation:");

      for (var i = 0; i < explanation.Count; i++)
      {
        var step = explanation[i];

        Console.WriteLine($"\t{i + 1} - {step}");
      }
    }
  }
}