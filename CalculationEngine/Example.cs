﻿using System;
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
    public static CompiledCalculation Calculation { get; } = CompiledCalculation.Create(() =>
    {
      var ordinaryEarnings = YTD(OrdinaryEarnings);
      var allowances       = YTD(Allowances);
      var deductions       = YTD(Deductions);
      var leave            = YTD(Leave);

      var totalEarnings = Variable("A", Sum(ordinaryEarnings, allowances, deductions, leave));

      var b = Variable("B", Round(totalEarnings - allowances), includeLabel: true);
      var c = Variable("C", Round(totalEarnings - deductions), includeLabel: true);
      var d = Variable("D", Round(totalEarnings - leave), includeLabel: true);

      var e = Variable("E", Truncate(Tax(PAYG, totalEarnings)), includeLabel: true);

      return b + c - d * e / 2m;
    });

    public static void Main()
    {
      Execute(Calculation);
    }

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