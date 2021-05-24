using CalculationEngine.Model.Nodes;

namespace CalculationEngine.Tests.Model.Nodes
{
  public sealed class VariableExpressionTests : ExpressionTestCase
  {
    protected override int ExpectedSteps => 1;

    internal override CalculationExpression BuildCalculation() => new VariableExpression(
      Symbol: "A",
      Operand: new ConstantExpression(10_000m),
      IncludeLabel: true
    );
  }
}