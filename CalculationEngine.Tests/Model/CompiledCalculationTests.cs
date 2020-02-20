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
      var earnings = YTD(Earnings);
      var tax      = Tax(PAYG, earnings);

      return Round(Truncate(YTD(All) - tax));
    });

    [Fact]
    public void it_should_compile_a_valid_calculation()
    {
      var output = Calculation.Execute(new EvaluationContext());

      Assert.True(output > 0m);
    }
  }
}