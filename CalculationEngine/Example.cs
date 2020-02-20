using System;
using CalculationEngine.Model;
using CalculationEngine.Model.Evaluation;
using static CalculationEngine.Model.Calculation;
using static CalculationEngine.Model.Evaluation.EarningsCategory;
using static CalculationEngine.Model.Evaluation.TaxCategory;

namespace CalculationEngine
{
  public static class Example
  {
    /// <summary>Statically declare calculations, and compile them once at application start-up.</summary>
    public static CompiledCalculation Calculation1 { get; } = CompiledCalculation.Create(() =>
    {
      var earnings   = YTD(Earnings);
      var allowances = YTD(Allowances);
      var deductions = YTD(Deductions);
      var leave      = YTD(Leave);

      var allEarnings = Label("Σ YTD", Sum(earnings, allowances, deductions, leave));

      var a = Label("A", Round(earnings - allowances - deductions - leave));
      var b = Label("B", Truncate(Tax(PAYG, allEarnings)));

      return a + b;
    });

    public static void Main() => Execute(Calculation1);

    private static void Execute(CompiledCalculation calculation)
    {
      var context = new EvaluationContext();

      var figure1     = calculation.Interpet(context);
      var figure2     = calculation.Execute(context);
      var explanation = calculation.Explain(context);

      Console.WriteLine($"Interpret: {figure1}");
      Console.WriteLine($"Execute: {figure2}");

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