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
  public sealed class CalculationBehaviourTests
  {
    [Theory]
    [MemberData(nameof(GetCalculationsUnderTest))]
    public void it_should_interpret_all_calculations(Calculation calculation)
    {
      calculation.Evaluate(new EvaluationContext());
    }

    [Theory]
    [MemberData(nameof(GetCalculationsUnderTest))]
    public void it_should_explain_all_calculations(Calculation calculation)
    {
      Assert.NotNull(calculation.Explain(new EvaluationContext()));
    }

    public static IEnumerable<object[]> GetCalculationsUnderTest()
    {
      yield return Example(() =>
      {
        return 100m + 200m / 3m;
      });

      yield return Example(() =>
      {
        var earnings = YTD(All);

        return earnings / 2m;
      });

      yield return Example(() =>
      {
        var a = Variable("A", 4000m);
        var b = Variable("B", 2000m);

        return a / b;
      });

      yield return Example(() =>
      {
        var earnings = YTD(All);
        var tax      = Tax(PAYG, earnings);

        return Round(earnings - Truncate(tax));
      });

      yield return Example(() =>
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

      static object[] Example(Func<Calculation> factory) => new object[] {Create(factory)};
    }
  }
}