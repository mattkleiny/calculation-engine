using CalculationEngine.Model.Nodes;

namespace CalculationEngine.Tests.Model.Nodes
{
  public sealed class SigmaExpressionTests : ExpressionTestCase
  {
    protected override decimal? ExpectedOutput => 35_000m;

    internal override CalculationExpression BuildCalculation() => new SigmaExpression(new[]
    {
      new ConstantExpression(10_000m),
      new ConstantExpression(20_000m),
      new ConstantExpression(5_000m)
    });
  }
}