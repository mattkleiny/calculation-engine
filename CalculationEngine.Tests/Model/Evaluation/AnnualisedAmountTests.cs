using CalculationEngine.Model.Evaluation;
using Xunit;

namespace CalculationEngine.Tests.Model.Evaluation
{
  public class AnnualisedAmountTests
  {
    [Fact]
    public void it_should_calculate_correctly()
    {
      var amount = new AnnualisedAmount(60_000m, 12m);

      Assert.Equal(amount, 5000m);
    }

    [Fact]
    public void it_should_reverse_calculate_correctly()
    {
      var amount = AnnualisedAmount.FromAnnualised(5000m, 12m);

      Assert.Equal(amount.Total, 60_000m);
      Assert.Equal(amount.Periods, 12m);
    }
  }
}