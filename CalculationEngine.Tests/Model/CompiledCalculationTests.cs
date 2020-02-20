using System;
using System.Collections.Generic;
using CalculationEngine.Model;
using CalculationEngine.Model.Evaluation;
using Xunit;
using static CalculationEngine.Model.Calculation;
using static CalculationEngine.Model.Evaluation.EarningsCategory;
using static CalculationEngine.Model.Evaluation.TaxCategory;

namespace CalculationEngine.Tests.Model
{
  public sealed class CompiledCalculationTests
  {
    public static readonly CompiledCalculation Calculation = CompiledCalculation.Create(() =>
    {
      var earnings = YTD(OrdinaryEarnings);
      var tax      = Tax(PAYG, earnings);

      return Round(Truncate(YTD(All) - tax));
    });

    [Fact]
    public void it_should_compile_a_valid_calculation()
    {
      var output = Calculation.Execute(new EvaluationContext());

      Assert.True(output > 0m);
    }

    [Theory]
    [MemberData(nameof(GetCalculationsUnderTest))]
    public void it_should_evaluate_the_same_interpreted_as_compiled(CompiledCalculation calculation)
    {
      var context = new EvaluationContext();

      var result1 = calculation.Interpet(context);
      var result2 = calculation.Execute(context);

      Assert.Equal(result1, result2);
    }

    public static IEnumerable<object[]> GetCalculationsUnderTest()
    {
      yield return TestCalculation(() =>
      {
        return 100m + 200m / 3m;
      });

      yield return TestCalculation(() =>
      {
        var earnings = YTD(All);

        return earnings / 2m;
      });

      yield return TestCalculation(() =>
      {
        var a = Variable("A", 4000m);
        var b = Variable("B", 2000m);

        return a / b;
      });

      yield return TestCalculation(() =>
      {
        var earnings = YTD(All);
        var tax      = Tax(PAYG, earnings);

        return Round(earnings - Truncate(tax));
      });

      yield return TestCalculation(() =>
      {
        var ordinaryEarnings = YTD(OrdinaryEarnings);
        var allowances       = YTD(Allowances);
        var deductions       = YTD(Deductions);
        var leave            = YTD(Leave);

        var a = Variable("A", Sum(ordinaryEarnings, allowances, deductions, leave));

        var b = Variable("B", Round(a - allowances));
        var c = Variable("C", Round(a - deductions));
        var d = Variable("D", Round(a - leave));

        var e = Variable("E", Truncate(Tax(PAYG, a)));

        return b + c - d * e / 2m;
      });
    }

    private static object[] TestCalculation(Func<Calculation> factory) => new object[] { CompiledCalculation.Create(factory) };
  }
}