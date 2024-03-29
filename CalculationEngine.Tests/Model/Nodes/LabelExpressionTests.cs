using CalculationEngine.Model.Nodes;

namespace CalculationEngine.Tests.Model.Nodes
{
  public sealed class LabelExpressionTests : ExpressionTestCase
  {
    protected override decimal? ExpectedOutput => 10_000m;
    protected override int      ExpectedSteps  => 1;

    internal override CalculationExpression BuildCalculation() => new LabelExpression(
      Label: "A",
      Expression: new ConstantExpression(10_000m)
    );
  }
}