using CalculationEngine.Model.Nodes;

namespace CalculationEngine.Tests.Model.Nodes
{
  public sealed class ConstantExpressionTests : ExpressionTestCase
  {
    protected override decimal? ExpectedOutput => 10_000m;

    internal override CalculationExpression BuildCalculation() => new ConstantExpression(10_000m);
  }
}