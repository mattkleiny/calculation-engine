using CalculationEngine.Model.Evaluation;
using Xunit;

namespace CalculationEngine.Tests.Model.Evaluation
{
  public class FixedRateTests
  {
    [Fact]
    public void it_should_calculate_correctly()
    {
      var rate = new FixedRate(20m, 100m);

      Assert.Equal(2000m, rate);
    }

    [Fact]
    public void it_should_reverse_calculate_correctly()
    {
      var rate = FixedRate.FromFixedRate(20_000m, 10m);

      Assert.Equal(2000m, rate.Amount);
      Assert.Equal(10m, rate.Rate);
    }
  }
}