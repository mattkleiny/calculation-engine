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
    public static CompiledCalculation ExampleCalculation { get; } = CompiledCalculation.Create(() =>
    {
      var earnings   = YTD(Earnings);
      var allowances = YTD(Allowances);
      var deductions = YTD(Deductions);
      var leave      = YTD(Leave);

      var allEarnings = Sum(earnings, allowances, deductions, leave);

      return Round(earnings - allowances - deductions - leave) + Truncate(Tax(PAYG, allEarnings));
    });

    public static void Main()
    {
      var context = new EvaluationContext();

      var figure1 = ExampleCalculation.Interpet(context);
      var figure2 = ExampleCalculation.Execute(context);

      Console.WriteLine(figure1);
      Console.WriteLine(figure2);
    }
  }
}