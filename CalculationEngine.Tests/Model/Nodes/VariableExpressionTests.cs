using CalculationEngine.Model.Nodes;

namespace CalculationEngine.Tests.Model.Nodes
{
  public sealed class VariableExpressionTests : ExpressionTestCase
  {
    protected override int ExpectedSteps => 1;

    internal override CalculationExpression Build() => new VariableExpression(
      symbol: "A",
      operand: new ConstantExpression(10_000m),
      includeLabel: true
    );
  }
}